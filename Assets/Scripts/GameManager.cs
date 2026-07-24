using System;
using System.Collections;
using UnityEngine;

public enum GameState { Menu, Playing, Results }

public class GameManager : MonoBehaviour
{   
    [Header("Game")]
    public int gameDifficulty = 1;
    public int winSpeed = 150;

    [Header("System References")]
    [SerializeField] private SpeedBar speedBar;
    [SerializeField] private HermesController hermes;
    [SerializeField] private GameTimer timer;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private LeaderboardManager leaderboard;

    [Header("UI Screens")]
    [SerializeField] private GameObject menuScreen;
    [SerializeField] private GameObject hudScreen;
    [SerializeField] private GameObject resultsScreen;
    [SerializeField] private GameObject nameEntryScreen;
    [SerializeField] private GameObject leaderboardScreen;



    public GameState State { get; private set; }
    public float FinalAveragePPS { get; private set; }

    void OnEnable()
    {
        GameTimer.OnGameEnd += HandleGameEnd;
    }

    void OnDisable()
    {
        GameTimer.OnGameEnd -= HandleGameEnd;
    }

    void Start()
    {
        State = GameState.Menu;
        ShowOnly(menuScreen);
    }

    public void StageClear()
    {
        timer.StopTimer();
        gameDifficulty += 1;
        Debug.Log("Hermes reached the end! Increasing the difficulty to " + gameDifficulty);
        StartGame(false);
    }

    public void StartGame(Boolean restartGame)
    {
        StartCoroutine(ContinueGame(restartGame));
    }

    void HandleGameEnd()
    {
        speedBar.GameActive = false;
        hermes.GameActive = false;

        FinalAveragePPS = scoreManager.AveragePPS();
        scoreManager.GameActive = false;

        State = GameState.Results;
        ShowOnly(nameEntryScreen);
    }

    public void SubmitName(string playerName)
    {
        leaderboard.AddEntry(playerName, scoreManager.Score, scoreManager.TotalPresses, hermes.DistanceTraveled);
        ShowOnly(resultsScreen);
    }

    public void PlayAgain()
    {
        StartGame(true);
    }

    public void ShowMainMenu()
    {
        State = GameState.Menu;
        ShowOnly(menuScreen);
    }

    public void ShowLeaderboard()
    {
        ShowOnly(leaderboardScreen);
    }

    void ShowOnly(GameObject screen)
    {
        SetActiveIfAssigned(menuScreen, menuScreen == screen);
        SetActiveIfAssigned(hudScreen, hudScreen == screen);
        SetActiveIfAssigned(resultsScreen, resultsScreen == screen);
        SetActiveIfAssigned(nameEntryScreen, nameEntryScreen == screen);
        SetActiveIfAssigned(leaderboardScreen, leaderboardScreen == screen);
    }

    void SetActiveIfAssigned(GameObject screen, bool active)
    {
        if (screen != null) screen.SetActive(active);
    }

    IEnumerator ContinueGame(bool restartGame)
    {
        speedBar.GameActive = false;
        hermes.GameActive = false;
        scoreManager.GameActive = false;
        if (restartGame) 
        {
            gameDifficulty = 1;
            scoreManager.ResetScore();
        } 
        else
        {
            yield return new WaitForSeconds(3);
        }
        hermes.ResetHermes();

        // countdown here

        speedBar.ResetSpeed();
        speedBar.GameActive = true;
        hermes.GameActive = true;
        scoreManager.GameActive = true;

        timer.StartTimer();
        State = GameState.Playing;

        ShowOnly(hudScreen);
    }
}

