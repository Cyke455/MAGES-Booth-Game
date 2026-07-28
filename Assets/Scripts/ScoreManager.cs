using System;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public event Action<int> OnStageBonusAwarded;
    public event Action OnComboEngaged;

    [Header("References")]
    [SerializeField] private HermesController hermes;
    [SerializeField] private TMP_Text scoreText;

    [Header("Scoring Weights")]
    [SerializeField] private float pointsPerPress = 3f;
    [SerializeField] private float comboBonusThresholdPPS = 6f;
    [SerializeField] private float comboBonusPointsPerPress = 1f;

    [Header("Stage Clear Reward")]
    [SerializeField] private float goalBonusBase = 30f;
    [SerializeField] private float goalBonusPerDifficulty = 5f;
    [SerializeField] private float maxTimeBonus = 60f;

    public bool GameActive = true;
    public int TotalPresses { get; private set; }
    public int Score { get; private set; }
    public int StagesCleared { get; private set; }

    private float roundStartTime;
    private float bankedDistance;
    private float bankedBonus;

    private int pressesThisStage;
    private float stageStartTime;
    private bool comboActiveThisStage;

    void OnEnable()
    {
        SpeedBar.OnKeyPressed += HandlePress;
    }

    void OnDisable()
    {
        SpeedBar.OnKeyPressed -= HandlePress;
    }

    void Update()
    {
        if (GameActive) RecalculateScore();
    }

    void HandlePress()
    {
        if (!GameActive) return;

        TotalPresses++;
        pressesThisStage++;

        if (!comboActiveThisStage && StagePPS() >= comboBonusThresholdPPS)
        {
            comboActiveThisStage = true;
            OnComboEngaged?.Invoke();
        }
    }

    void RecalculateScore()
    {
        float score = TotalPresses * pointsPerPress + bankedBonus;

        Score = Mathf.RoundToInt(score);
        if (scoreText != null) scoreText.text = Score.ToString();
    }

    public float AveragePPS()
    {
        float elapsed = Mathf.Max(Time.time - roundStartTime, 0.0001f);
        return TotalPresses / elapsed;
    }

    float StagePPS()
    {
        float elapsed = Mathf.Max(Time.time - stageStartTime, 0.0001f);
        return pressesThisStage / elapsed;
    }

    public void ResetScore()
    {
        TotalPresses = 0;
        Score = 0;
        StagesCleared = 0;
        bankedDistance = 0f;
        bankedBonus = 0f;
        roundStartTime = Time.time;
        if (scoreText != null) scoreText.text = "0";
    }

    public void OnStageStart()
    {
        pressesThisStage = 0;
        stageStartTime = Time.time;
        comboActiveThisStage = false;
    }

    public void AwardStageClear(float timeRemaining, float roundDuration, int difficultyAtClear)
    {
        float goalBonus = goalBonusBase + goalBonusPerDifficulty * difficultyAtClear;
        float timeFraction = roundDuration > 0f ? Mathf.Clamp01(timeRemaining / roundDuration) : 0f;
        float timeBonus = maxTimeBonus * timeFraction;

        float stageBonus = goalBonus + timeBonus;
        if (comboActiveThisStage)
        {
            stageBonus += pressesThisStage * comboBonusPointsPerPress;
        }

        bankedBonus += stageBonus;
        StagesCleared++;

        OnStageBonusAwarded?.Invoke(Mathf.RoundToInt(stageBonus));
    }

    public void BankDistance()
    {
        if (hermes != null) bankedDistance += hermes.DistanceTraveled;
    }
}
