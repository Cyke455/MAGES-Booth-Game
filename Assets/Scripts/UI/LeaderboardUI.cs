using UnityEngine;

public class LeaderboardUI : MonoBehaviour
{
    [SerializeField] private LeaderboardManager leaderboard;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Transform contentParent;
    [SerializeField] private LeaderboardRowUI rowPrefab;

    private GameMode currentMode;

    void OnEnable()
    {
        currentMode = gameManager != null ? gameManager.CurrentMode : GameMode.SoloSprint;
        Populate();
    }

    public void ShowSoloSprint()
    {
        currentMode = GameMode.SoloSprint;
        Populate();
    }

    public void ShowRelayRace()
    {
        currentMode = GameMode.RelayRace;
        Populate();
    }

    void Populate()
    {
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        int rank = 1;
        foreach (var entry in leaderboard.GetEntries(currentMode))
        {
            LeaderboardRowUI row = Instantiate(rowPrefab, contentParent);
            row.Populate(rank, entry);
            rank++;
        }
    }
}
