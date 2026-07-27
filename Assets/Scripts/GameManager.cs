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
    [SerializeField] private CountdownDisplay countdownDisplay;

    [Header("UI Screens")]
    [SerializeField] private GameObject menuScreen;
    [SerializeField] private GameObject hudScreen;
    [SerializeField] private GameObject resultsScreen;
    [SerializeField] private GameObject nameEntryScreen;
    [SerializeField] private GameObject leaderboardScreen;



    public GameState State { get; private set; }
    public float FinalAveragePPS { get; private set; }

    private bool isTransitioning;
    private bool nameSubmitted;

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
        SoundManager.Instance?.PlayMusic(SoundType.Theme);
    }

    public void StageClear()
    {
        timer.StopTimer();

        Debug.Log("Hermes reached the end! Increasing the difficulty to " + (gameDifficulty+1));
    
        StartGame(false);
    }

    public void StartGame(Boolean restartGame)
    {
        if (isTransitioning) return;

        StartCoroutine(ContinueGame(restartGame));
    }

    void HandleGameEnd()
    {
        speedBar.GameActive = false;
        hermes.GameActive = false;

        FinalAveragePPS = scoreManager.AveragePPS();
        scoreManager.GameActive = false;

        nameSubmitted = false;
        State = GameState.Results;
        ShowOnly(nameEntryScreen);
        SoundManager.Instance?.CrossFadeMusic(SoundType.Theme, 1f);
    }

    public void SubmitName(string playerName)
    {
        if (nameSubmitted || State != GameState.Results) return;
        nameSubmitted = true;

        leaderboard.AddEntry(playerName, scoreManager.Score, scoreManager.TotalPresses, hermes.DistanceTraveled);
        ShowOnly(resultsScreen);
    }

    public void PlayAgain()
    {
        StartGame(true);
    }

    public void ShowMainMenu()
    {
        if (isTransitioning) return;

        State = GameState.Menu;
        ShowOnly(menuScreen);
        SoundManager.Instance?.PlayMusic(SoundType.Theme);
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
        isTransitioning = true;
        try
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
                gameDifficulty += 1;
            }
            hermes.ResetHermes();

            ShowOnly(hudScreen);

            speedBar.ResetSpeed();

            countdownDisplay.DisplayNumber(0);
            yield return new WaitForSeconds(1);
            countdownDisplay.DisplayNumber(1);
            yield return new WaitForSeconds(1);
            countdownDisplay.DisplayNumber(2);
            yield return new WaitForSeconds(1);
            countdownDisplay.DisplayStart();
            
            speedBar.GameActive = true;
            hermes.GameActive = true;
            scoreManager.GameActive = true;
            timer.StartTimer();
            State = GameState.Playing;

            
            SoundManager.Instance?.CrossFadeMusic(SoundType.GameplayTheme, 1f);
        }
        finally
        {
            isTransitioning = false;
        }
    }
}

