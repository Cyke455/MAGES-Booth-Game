using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.UI;

public class BarUI : MonoBehaviour
{
    private Vector2 velocity = Vector2.zero;

    float SpeedValue;

    [SerializeField] private Image SpeedMeterBar;

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
        float barWidth = (SpeedValue / 100); // 100 is max

        RectTransform rectTransform = SpeedMeterBar.GetComponent<RectTransform>();
        Vector2 currentBarSize = new Vector2(rectTransform.rect.width, rectTransform.rect.height);
        SpeedMeterBar.rectTransform.sizeDelta = Vector2.SmoothDamp(currentBarSize, new Vector2(barWidth * 400, 70), ref velocity, 0.1f);
    }
}
