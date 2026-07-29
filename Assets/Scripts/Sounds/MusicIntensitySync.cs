using UnityEngine;

public class MusicIntensitySync : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private float maxIntensityMultiplier = 1.5f;

    private float currentSpeedValue;

    void OnEnable()
    {
        SpeedBar.OnSpeedUpdate += HandleSpeedUpdate;
    }

    void OnDisable()
    {
        SpeedBar.OnSpeedUpdate -= HandleSpeedUpdate;
    }

    void HandleSpeedUpdate(float newValue, bool instantUpdate)
    {
        currentSpeedValue = newValue;
    }

    void Update()
    {
        if (SoundManager.Instance == null || gameManager == null) return;

        float t = Mathf.Clamp01(currentSpeedValue / gameManager.winSpeed);
        float intensity = Mathf.Lerp(1f, maxIntensityMultiplier, t);

        SoundManager.Instance.SetMusicIntensity(intensity);
    }
}
