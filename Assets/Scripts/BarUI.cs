using UnityEngine;
using UnityEngine.UI;

public class BarUI : MonoBehaviour
{
    float SpeedValue;
    float MaxValue;

    
    [SerializeField] private float barSpeed;
    [SerializeField] private Image SpeedMeterBar;

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
        MaxValue = 150; // temp
        float barWidth = (SpeedValue / MaxValue); 

        RectTransform rectTransform = SpeedMeterBar.GetComponent<RectTransform>();
        //Vector2 currentBarSize = new Vector2(rectTransform.rect.width, rectTransform.rect.height);

        float alpha = 1f - Mathf.Exp(-Time.deltaTime / barSpeed);
        SpeedMeterBar.rectTransform.sizeDelta = Vector2.Lerp(rectTransform.rect.size, new Vector2(barWidth * 400, rectTransform.rect.height), alpha);
    }
}
