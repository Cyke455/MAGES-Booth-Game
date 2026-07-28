using System.Collections.Generic;
using UnityEngine;

public class MusicIntensitySync : MonoBehaviour
{
    [SerializeField] private float windowSeconds = 3f;
    [SerializeField] private float pressesPerSecondForMaxIntensity = 6f;
    [SerializeField] private float maxIntensityMultiplier = 1.2f;

    private readonly Queue<float> pressTimestamps = new Queue<float>();

    void OnEnable()
    {
        SpeedBar.OnKeyPressed += HandleKeyPressed;
    }

    void OnDisable()
    {
        SpeedBar.OnKeyPressed -= HandleKeyPressed;
    }

    void HandleKeyPressed()
    {
        pressTimestamps.Enqueue(Time.time);
    }

    void Update()
    {
        while (pressTimestamps.Count > 0 && Time.time - pressTimestamps.Peek() > windowSeconds)
        {
            pressTimestamps.Dequeue();
        }

        if (SoundManager.Instance == null) return;

        float pressesPerSecond = pressTimestamps.Count / windowSeconds;
        float t = Mathf.Clamp01(pressesPerSecond / pressesPerSecondForMaxIntensity);
        float intensity = Mathf.Lerp(1f, maxIntensityMultiplier, t);

        SoundManager.Instance.SetMusicIntensity(intensity);
    }
}
