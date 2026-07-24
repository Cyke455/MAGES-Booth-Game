using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[Serializable]
public class LeaderboardEntry
{
    public string playerName;
    public int score;
    public int totalPresses;
    public float distanceTraveled;
    public string date;
}

[Serializable]
public class LeaderboardData
{
    public List<LeaderboardEntry> entries = new List<LeaderboardEntry>();
}

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField] private int maxEntries = 10;
    [SerializeField] private string fileName = "leaderboard.json";

    private LeaderboardData data = new LeaderboardData();

    public IReadOnlyList<LeaderboardEntry> Entries => data.entries;

    private string FilePath => Path.Combine(Application.persistentDataPath, fileName);

    void Awake()
    {
        Load();
    }

    public void AddEntry(string playerName, int score, int totalPresses, float distanceTraveled)
    {
        data.entries.Add(new LeaderboardEntry
        {
            playerName = string.IsNullOrWhiteSpace(playerName) ? "Player" : playerName,
            score = score,
            totalPresses = totalPresses,
            distanceTraveled = distanceTraveled,
            date = DateTime.Now.ToString("yyyy-MM-dd HH:mm")
        });

        data.entries = data.entries
            .OrderByDescending(e => e.score)
            .Take(maxEntries)
            .ToList();

        Save();
    }

    void Load()
    {
        if (!File.Exists(FilePath)) return;

        string json = File.ReadAllText(FilePath);
        data = JsonUtility.FromJson<LeaderboardData>(json) ?? new LeaderboardData();
    }

    void Save()
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(FilePath, json);
    }
}
