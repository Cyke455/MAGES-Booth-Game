using UnityEngine;
using UnityEngine.UI;

public class BarUI : MonoBehaviour
{
    float SpeedValue;

    [SerializeField] private GameManager gameManager;

    [SerializeField] private float barSpeed;
    [SerializeField] private Image SpeedMeterBar;
    [SerializeField] private float MaxValue;
    [SerializeField] private float barWidthPixels = 1200;

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
        UpdateBarMeter();
    }

    void UpdateBarValue(float newValue)
    {
        SpeedValue = newValue;
    }

    void UpdateBarMeter()
    {
        MaxValue = gameManager.winSpeed;

        float barWidth = barWidthPixels - (SpeedValue / MaxValue) * barWidthPixels;
        RectTransform rectTransform = SpeedMeterBar.GetComponent<RectTransform>();
        float alpha = 1f - Mathf.Exp(-Time.deltaTime / barSpeed);

        SpeedMeterBar.rectTransform.sizeDelta = Vector2.Lerp(rectTransform.rect.size, new Vector2(barWidth, rectTransform.rect.height), alpha);
    }
}
