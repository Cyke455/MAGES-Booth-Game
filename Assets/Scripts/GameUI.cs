using System;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    float SpeedValue;

    [SerializeField] private GameManager gameManager;

    [SerializeField] private float barSpeed;
    [SerializeField] private Color barColor;
    [SerializeField] private Image SpeedMeterBar;
    [SerializeField] private float MaxValue;
    [SerializeField] private float barWidthPixels = 1200;
    [SerializeField] private Transform shakingElements;

    private float shakeClock;

    void OnEnable()
    {
        SpeedBar.OnSpeedUpdate += UpdateBarValue;
    }
    void OnDisable()
    {
        SpeedBar.OnSpeedUpdate -= UpdateBarValue;
    }

    void Update()
    {
        UpdateBarMeter(false);
        float shakeSpeed = 6f;
        float shakeIntensity = 2f;
        shakeClock += Time.deltaTime * shakeSpeed * (1 + (gameManager.gameDifficulty - 1)*0.1f);
        shakingElements.localPosition = new Vector2(Mathf.Sin(shakeClock) * shakeIntensity, Mathf.Sin(shakeClock * 1.25f) * shakeIntensity);
        shakingElements.rotation = Quaternion.Euler(0, 0, Mathf.Cos(shakeClock * 0.3f)/5);
    }

    void UpdateBarValue(float newValue, bool instantUpdate)
    {
        if (newValue > SpeedValue)
        {
            SpeedMeterBar.color = barColor + new Color(0.2f, 0.2f, 0.2f);
        }
        SpeedValue = newValue;
        

        if (instantUpdate) UpdateBarMeter(true);
    }

    void UpdateBarMeter(bool instantUpdate)
    {
        MaxValue = gameManager.winSpeed;
        float colorAlpha = Mathf.Clamp(1f - Mathf.Exp(-Time.deltaTime / 0.2f), 0, 1);
        SpeedMeterBar.color = Color.Lerp(SpeedMeterBar.color, barColor, colorAlpha);

        float barWidth = barWidthPixels - (SpeedValue / MaxValue) * barWidthPixels;
        RectTransform rectTransform = SpeedMeterBar.GetComponent<RectTransform>();
        float alpha = Math.Clamp(1f - Mathf.Exp(-Time.deltaTime / barSpeed), 0, 1);


        if (instantUpdate) alpha = 1;

        SpeedMeterBar.rectTransform.sizeDelta = Vector2.Lerp(rectTransform.rect.size, new Vector2(barWidth, rectTransform.rect.height), alpha);
    }
}
