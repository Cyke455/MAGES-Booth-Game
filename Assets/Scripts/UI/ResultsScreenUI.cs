using TMPro;
using UnityEngine;

public class ResultsScreenUI : MonoBehaviour
{
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private HermesController hermes;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private LeaderboardManager leaderboard;

    [SerializeField] private TMP_Text finalScoreText;
    [SerializeField] private TMP_Text totalPressesText;
    [SerializeField] private TMP_Text distanceText;
    [SerializeField] private TMP_Text avgPpsText;
    [SerializeField] private TMP_Text bestScoreText;

    void OnEnable()
    {
        finalScoreText.text = scoreManager.Score.ToString();
        totalPressesText.text = scoreManager.TotalPresses.ToString();
        distanceText.text = hermes.DistanceTraveled.ToString("F1");
        avgPpsText.text = gameManager.FinalAveragePPS.ToString("F2");

        int best = 0;
        foreach (var entry in leaderboard.Entries)
        {
            best = Mathf.Max(best, entry.score);
        }
        bestScoreText.text = best.ToString();
    }
}
