using System;
using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public static event Action OnGameEnd;

    [SerializeField] private float roundDuration = 45f;
    [SerializeField] private TMP_Text timerText;

    public float TimeRemaining { get; private set; }
    public bool IsRunning { get; private set; }
    public float RoundDuration => roundDuration;

    public void SetRoundDuration(float duration)
    {
        roundDuration = duration;
    }

    public void StartTimer(bool reset = true)
    {
        if (reset) TimeRemaining = roundDuration;
        IsRunning = true;
        UpdateDisplay();
    }

    public void StopTimer()
    {
        IsRunning = false;
    }

    void Update()
    {
        if (!IsRunning) return;

        TimeRemaining = Mathf.Max(TimeRemaining - Time.deltaTime, 0f);
        UpdateDisplay();

        if (TimeRemaining <= 0f)
        {
            IsRunning = false;
            OnGameEnd?.Invoke();
        }
    }

    void UpdateDisplay()
    {
        if (timerText != null)
        {
            timerText.text = Mathf.CeilToInt(TimeRemaining).ToString();
        }
    }
}
