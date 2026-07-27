using UnityEngine;

public class LeaderboardUI : MonoBehaviour
{
    [SerializeField] private LeaderboardManager leaderboard;
    [SerializeField] private Transform contentParent;
    [SerializeField] private LeaderboardRowUI rowPrefab;

    void OnEnable()
    {
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        int rank = 1;
        foreach (var entry in leaderboard.Entries)
        {
            LeaderboardRowUI row = Instantiate(rowPrefab, contentParent);
            row.Populate(rank, entry);
            rank++;
        }
    }
}
