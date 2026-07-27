using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private HermesController hermes;
    [SerializeField] private TMP_Text scoreText;

    [Header("Scoring Weights")]
    [SerializeField] private float pointsPerPress = 3f;
    [SerializeField] private float pointsPerDistance = 2f;
    [SerializeField] private float comboBonusThresholdPPS = 6f;
    [SerializeField] private float comboBonusPointsPerPress = 1f;

    public bool GameActive = true;
    public int TotalPresses { get; private set; }
    public int Score { get; private set; }

    private float roundStartTime;

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
    }

    void RecalculateScore()
    {
        float distance = hermes != null ? hermes.DistanceTraveled : 0f;
        float score = TotalPresses * pointsPerPress + distance * pointsPerDistance;

        if (AveragePPS() >= comboBonusThresholdPPS)
        {
            score += TotalPresses * comboBonusPointsPerPress;
        }

        Score = Mathf.RoundToInt(score);
        if (scoreText != null) scoreText.text = Score.ToString();
    }

    public float AveragePPS()
    {
        float elapsed = Mathf.Max(Time.time - roundStartTime, 0.0001f);
        return TotalPresses / elapsed;
    }

    public void ResetScore()
    {
        TotalPresses = 0;
        Score = 0;
        roundStartTime = Time.time;
        if (scoreText != null) scoreText.text = "0";
    }
}
