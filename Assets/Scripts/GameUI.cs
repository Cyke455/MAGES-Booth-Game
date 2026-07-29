using System;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    float SpeedValue;

    [SerializeField] private GameManager gameManager;

    [SerializeField] private float barSpeed;
    private Color barColor;
    [SerializeField] private Image SpeedMeterBar;
    [SerializeField] private float MaxValue;
    [SerializeField] private float barWidthPixels = 1200;
    [SerializeField] private Transform shakingElements;

    [Header("Speed Sync")]
    [SerializeField] private float minSpeedFactor = 0.5f;
    [SerializeField] private float maxSpeedFactor = 1.5f;
    [SerializeField] private float speedSmoothing = 0.15f;

    private float shakeClock;
    private float speedFactor;

    void OnEnable()
    {
        SpeedBar.OnSpeedUpdate += UpdateBarValue;
    }
    void OnDisable()
    {
        SpeedBar.OnSpeedUpdate -= UpdateBarValue;
    }

    void Start()
    {
        barColor = SpeedMeterBar.color;
    }

    void Update()
    {
        UpdateBarMeter(false);
        float shakeSpeed = 10f;
        float shakeIntensity = 2f;
        float targetSpeedFactor = Mathf.Lerp(minSpeedFactor, maxSpeedFactor, Mathf.Clamp01(SpeedValue / gameManager.winSpeed));
        float speedAlpha = 1f - Mathf.Exp(-Time.deltaTime / speedSmoothing);
        speedFactor = Mathf.Lerp(speedFactor, targetSpeedFactor, speedAlpha);
        shakeClock += Time.deltaTime * shakeSpeed * speedFactor * (1 + (gameManager.gameDifficulty - 1)*0.1f);
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
