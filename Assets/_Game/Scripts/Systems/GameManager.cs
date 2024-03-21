using System;
using UnityEngine;

public class GameManager : Singleton<GameManager> {
    [SerializeField] private GameObject hookController;
    [SerializeField] private SpriteRenderer hookConstraints;
    [SerializeField] public Timer gameTimer;
    [SerializeField] public float gameLength = 60f;

    public bool paused = false;

    public GameState state = GameState.NONE;
    public static event Action<GameState> gameStateChanged;

    private void OnEnable() {
        gameTimer.timerExpired += OnTimerExpired;
    }

    private void OnDisable() {
        gameTimer.timerExpired -= OnTimerExpired;
    }

    private void Start() {
        UpdateGameState(GameState.MAIN_MENU);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            UnityEditor.EditorApplication.isPlaying = false;
        }
        if (Input.GetKeyDown(KeyCode.Q)) {
            UpdateGameState(GameState.MAIN_MENU);
        }
        if (Input.GetKeyDown(KeyCode.P)) {
            if (!paused) {
                Pause();
            } else {
                Unpause();
            }
        }
    }

    public void Pause() {
        if (state != GameState.GAME_START) return;
        paused = true;
        Time.timeScale = 0f;
    }

    public void Unpause() {
        paused = false;
        Time.timeScale = 1f;
    }

    public void UpdateGameState(GameState newState) {
        if (newState == state) return;
        state = newState;

        switch (newState) {
            case GameState.MAIN_MENU:
                // UISystem.Instance.ShowGameUI(false);
                // UISystem.Instance.ShowMainMenu(true);
                StartCoroutine(UISystem.Instance.FadeToMainMenu());
                FishManager.Instance.enabled = false;
                ScoreManager.Instance.enabled = false;
                hookController.SetActive(false);
                hookConstraints.enabled = false;
                gameTimer.StopTimer();
            break;
            case GameState.GAME_START:
                // UISystem.Instance.ShowMainMenu(false);
                // UISystem.Instance.ShowGameUI(true);
                FishManager.Instance.enabled = true;
                ScoreManager.Instance.enabled = true;
                hookController.SetActive(true);
                hookConstraints.enabled = true;
                gameTimer.StartTimer(gameLength);
            break;
            // case GameState.PAUSED:
            //     Time.timeScale = 0;
            // break;
            case GameState.SETTINGS_MENU:
            break;
            case GameState.GAME_OVER:
            break;
        }

        gameStateChanged?.Invoke(newState);
    }

    private void OnTimerExpired() {
        UpdateGameState(GameState.MAIN_MENU);
    }

    public string GetSecondsRemaining() {
        return gameTimer.SecondsToString();
    }
}