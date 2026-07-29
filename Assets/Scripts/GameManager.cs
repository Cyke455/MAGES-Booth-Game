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
    [SerializeField] private GameObject modeSelectScreen;
    [SerializeField] private GameObject hudScreen;
    [SerializeField] private GameObject resultsScreen;
    [SerializeField] private GameObject nameEntryScreen;
    [SerializeField] private GameObject leaderboardScreen;



    public GameState State { get; private set; }
    public GameMode CurrentMode { get; private set; }
    public float FinalAveragePPS { get; private set; }
    public int StagesCleared => scoreManager.StagesCleared;

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
        speedBar.GameActive = false;
        hermes.GameActive = false;
        SoundManager.Instance?.PlayMusic(SoundType.Theme);
    }

    public void StageClear()
    {
        timer.StopTimer();

        scoreManager.AwardStageClear(timer.TimeRemaining, timer.RoundDuration, gameDifficulty);

        Debug.Log("Hermes reached the end! Increasing the difficulty to " + (gameDifficulty+1));

        StartGame(false);

        SoundManager.Instance.PlaySFX(SoundType.Win);
    }

    public void ShowModeSelect()
    {
        if (isTransitioning) return;

        ShowOnly(modeSelectScreen);
    }

    public void StartSoloSprint()
    {
        CurrentMode = GameMode.SoloSprint;
        StartGame(true);
    }

    public void StartRelayRace()
    {
        CurrentMode = GameMode.RelayRace;
        StartGame(true);
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
        SoundManager.Instance.PlaySFX(SoundType.Lose);
        SoundManager.Instance?.CrossFadeMusic(SoundType.Theme, 1f);
    }

    public void SubmitName(string playerName)
    {
        if (nameSubmitted || State != GameState.Results) return;
        nameSubmitted = true;

        leaderboard.AddEntry(playerName, scoreManager.Score, scoreManager.TotalPresses, hermes.DistanceTraveled, CurrentMode);
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
        SetActiveIfAssigned(modeSelectScreen, modeSelectScreen == screen);
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
                scoreManager.BankDistance();
            }
            hermes.ResetHermes();

            ShowOnly(hudScreen);

            speedBar.ResetSpeed();

            countdownDisplay.DisplayNumber(0);
            SoundManager.Instance.PlaySFX(SoundType.Countdown);
            yield return new WaitForSeconds(1);
            countdownDisplay.DisplayNumber(1);
            SoundManager.Instance.PlaySFX(SoundType.Countdown);
            yield return new WaitForSeconds(1);
            countdownDisplay.DisplayNumber(2);
            SoundManager.Instance.PlaySFX(SoundType.Countdown);
            yield return new WaitForSeconds(1);
            countdownDisplay.DisplayStart();
            SoundManager.Instance.PlaySFX(SoundType.CountdownStart);

            speedBar.GameActive = true;
            hermes.GameActive = true;
            scoreManager.GameActive = true;
            scoreManager.OnStageStart();
            timer.StartTimer(restartGame || CurrentMode == GameMode.RelayRace);
            State = GameState.Playing;

            SoundManager.Instance?.CrossFadeMusic(SoundType.GameplayTheme, 1f);
        }
        finally
        {
            isTransitioning = false;
        }
    }
}

