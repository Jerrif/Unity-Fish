using System;
using UnityEngine;

public class GameManager : Singleton<GameManager> {
    [SerializeField] private GameObject hookController;
    [SerializeField] private SpriteRenderer hookConstraints;

    public GameState state = GameState.NONE;
    public static event Action<GameState> gameStateChanged;

    private void Start() {
        UpdateGameState(GameState.MAIN_MENU);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            // UnityEditor.EditorApplication.isPlaying = false;
            UpdateGameState(GameState.MAIN_MENU);
        }
    }

    // NOTE: READ THESE
    // https://www.reddit.com/r/Unity3D/comments/57e7np/how_many_scenes_do_your_games_typically_have/
    // https://www.reddit.com/r/Unity3D/comments/5ie9ow/should_i_use_different_scenes_for_menus/
    // https://forum.unity.com/threads/what-is-a-scene.481373/

    public void UpdateGameState(GameState newState) {
        if (newState == state) return;
        state = newState;

        switch (newState) {
            case GameState.MAIN_MENU:
                UISystem.Instance.ShowGameUI(false);
                UISystem.Instance.ShowMainMenu(true);
                FishManager.Instance.enabled = false;
                ScoreManager.Instance.enabled = false;
                hookController.SetActive(false);
                hookConstraints.enabled = false;
            break;
            case GameState.GAME_START:
                UISystem.Instance.ShowMainMenu(false);
                UISystem.Instance.ShowGameUI(true);
                FishManager.Instance.enabled = true;
                ScoreManager.Instance.enabled = true;
                hookController.SetActive(true);
                hookConstraints.enabled = true;
                HandleGameStart();
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

    private void HandleMainMenu() {
    }

    private void HandleGameStart() {
        
    }
}