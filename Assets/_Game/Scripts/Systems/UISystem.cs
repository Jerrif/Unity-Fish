using UnityEngine;

public class UISystem : Singleton<UISystem> {
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private ClickyButton playButton;
    [SerializeField] private GameObject gameUI;

    private void OnEnable() {
        GameManager.gameStateChanged += OnGameStateChanged;
    }

    private void OnDestroy() {
        GameManager.gameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newState) {
        print("waow new state dood! " + newState.ToString());
    //     // if (newState == GameState.MAIN_MENU) {
    //     //     Instantiate(mainMenuPanel);
    //     // }
    //     mainMenuPanel.SetActive(newState == GameState.MAIN_MENU);
    }

    public void PlayButtonPressed() {
        print("wow play");
        GameManager.Instance.UpdateGameState(GameState.GAME_START);
    }

    public void ShowGameUI(bool active) {
        gameUI.SetActive(active);
    }

    public void ShowMainMenu(bool active) {
        mainMenuUI.SetActive(active);
    }
}

// Game Manager - Controlling the flow of your game [Unity Tutorial]
// https://www.youtube.com/watch?v=4I0vonyqMi8
// https://www.youtube.com/watch?v=zJOxWmVveXU
// https://www.youtube.com/watch?v=tE1qH8OxO2Y