using System;
using System.Collections;
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
                StartCoroutine(HandleMainMenu());
            break;
            case GameState.GAME_START:
                StartCoroutine(HandleGameStart());
            break;
            case GameState.GAME_RUNNING:
            break;
            case GameState.SETTINGS_MENU:
            break;
            case GameState.GAME_OVER:
            break;
        }
        gameStateChanged?.Invoke(newState);
    }

    private IEnumerator HandleMainMenu() {
        gameTimer.StopTimer();
        yield return new WaitUntil(() => UISystem.Instance.FadeOut());
        UISystem.Instance.ShowGameUI(false);
        UISystem.Instance.ShowMainMenu(true);
        FishManager.Instance.enabled = false;
        ScoreManager.Instance.enabled = false;
        hookController.SetActive(false);
        hookConstraints.enabled = false;
        yield return new WaitForSecondsRealtime(0.5f);
        yield return new WaitUntil(() => UISystem.Instance.FadeIn());
    }

    private IEnumerator HandleGameStart() {
        FishManager.Instance.enabled = false; // hmm a little bit of a hack
        yield return new WaitUntil(() => UISystem.Instance.FadeOut());
        UISystem.Instance.ShowMainMenu(false);
        UISystem.Instance.InitGameUI();
        hookConstraints.enabled = true;
        yield return new WaitForSecondsRealtime(0.5f);
        yield return new WaitUntil(() => UISystem.Instance.FadeIn());
        FishManager.Instance.enabled = true;
        hookController.SetActive(true);
        gameTimer.StartTimer(gameLength);

        UpdateGameState(GameState.GAME_RUNNING);
    }

    // private IEnumerator HandleMainMenu() {

    // }

    private void OnTimerExpired() {
        UpdateGameState(GameState.MAIN_MENU);
    }

    public string GetSecondsRemaining() {
        return gameTimer.SecondsToString();
    }
}