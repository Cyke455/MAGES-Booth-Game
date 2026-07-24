using TMPro;
using UnityEngine;

public class LeaderboardRowUI : MonoBehaviour
{
    [SerializeField] private TMP_Text rankText;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text pressesText;
    [SerializeField] private TMP_Text distanceText;
    [SerializeField] private TMP_Text dateText;

    public void Populate(int rank, LeaderboardEntry entry)
    {
        rankText.text = rank.ToString();
        nameText.text = entry.playerName;
        scoreText.text = entry.score.ToString();
        pressesText.text = entry.totalPresses.ToString();
        distanceText.text = entry.distanceTraveled.ToString("F1");
        dateText.text = entry.date;
    }
}
