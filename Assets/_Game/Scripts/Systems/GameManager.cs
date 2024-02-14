using System;
using UnityEngine;

public class GameManager : Singleton<GameManager> {
    [SerializeField] float gameLength = 60f;
    // [SerializeField] Timer timer;

    public GameState state = GameState.NONE;
    public static event Action<GameState> gameStateChanged;

    private void OnEnable() {
        // timer.timerExpired += TimeIsUp;
    }

    private void Start() {
        UpdateGameState(GameState.MAIN_MENU);
        // timer.StartTimer(gameLength);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            // UnityEditor.EditorApplication.isPlaying = false;
            UpdateGameState(GameState.MAIN_MENU);
        }
    }

    private void TimeIsUp() {
        print("waow time is up");
        // SceneSystem.Instance.LoadScene(0);
    }

    public void UpdateGameState(GameState newState) {
        if (newState == state) return;
        state = newState;

        switch (newState) {
            case GameState.MAIN_MENU:
            break;
            case GameState.GAME_START:
            break;
            case GameState.PAUSED:
            break;
            case GameState.SETTINGS_MENU:
            break;
            case GameState.GAME_OVER:
            break;
        }

        gameStateChanged?.Invoke(newState);
    }
}

public enum GameState {
    MAIN_MENU,
    GAME_START,
    PAUSED,
    SETTINGS_MENU,
    GAME_OVER,
    NONE
}
