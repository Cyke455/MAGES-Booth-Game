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
    public string mode;
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

    public IReadOnlyList<LeaderboardEntry> GetEntries(GameMode mode)
    {
        string modeName = mode.ToString();
        return data.entries
            .Where(e => e.mode == modeName)
            .OrderByDescending(e => e.score)
            .ToList();
    }

    public void AddEntry(string playerName, int score, int totalPresses, float distanceTraveled, GameMode mode)
    {
        string modeName = mode.ToString();
        string resolvedName = string.IsNullOrWhiteSpace(playerName) ? "Player" : playerName;

        LeaderboardEntry existing = data.entries.FirstOrDefault(e =>
            e.mode == modeName && string.Equals(e.playerName, resolvedName, StringComparison.OrdinalIgnoreCase));

        if (existing != null && existing.score >= score)
        {
            // Existing score is already at least as high; keep it and don't add a duplicate.
            return;
        }

        if (existing != null)
        {
            data.entries.Remove(existing);
        }

        data.entries.Add(new LeaderboardEntry
        {
            playerName = resolvedName,
            score = score,
            totalPresses = totalPresses,
            distanceTraveled = distanceTraveled,
            date = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
            mode = modeName
        });

        List<LeaderboardEntry> topForMode = data.entries
            .Where(e => e.mode == modeName)
            .OrderByDescending(e => e.score)
            .Take(maxEntries)
            .ToList();

        data.entries = data.entries
            .Where(e => e.mode != modeName)
            .Concat(topForMode)
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
