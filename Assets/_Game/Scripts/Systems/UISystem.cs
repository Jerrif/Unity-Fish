using System;
using UnityEngine;
using UnityEngine.UI;

public class UISystem : Singleton<UISystem> {
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private Button playButton; // should probably not hardcode this? or just walk the mainMenuUI tree to find it
    [SerializeField] private GameObject gameUI;
    private ScreenTransition transitionController;

    private event Action afterFade;

    protected override void Awake() {
        base.Awake();
        transitionController = GetComponent<ScreenTransition>();
    }

    private void OnEnable() {
        GameManager.gameStateChanged += OnGameStateChanged;
        transitionController.fadeComplete += OnFadeComplete;
    }

    private void OnDestroy() {
        GameManager.gameStateChanged -= OnGameStateChanged;
        transitionController.fadeComplete -= OnFadeComplete;
    }

    private void OnGameStateChanged(GameState newState) {
    //     // if (newState == GameState.MAIN_MENU) {
    //     //     Instantiate(mainMenuPanel);
    //     // }
    //     mainMenuPanel.SetActive(newState == GameState.MAIN_MENU);
    }

    /* 
        play button pressed
        disable play button
        start fading out
        on fade out complete:
            unload old ui
            load new ui
        start fading in
        on fade in complete:
            update game state
        ??? profit

        I thought it'd be easy with the use of `Events`, and I kind of do have it working, but IMO it's pretty awful right now.
        I might try to refactor the `UIManager` to just use an update loop? Need to think about it a little more

    */

    public void PlayButtonPressed() {
        // GameManager.Instance.UpdateGameState(GameState.GAME_START);
        playButton.enabled = false; // IMPORTANT: remember to re-activate this after
        afterFade = FadeToGame;
        transitionController.FadeOut();
        // afterFade = GameManager.Instance.UpdateGameState(GameState.GAME_START);
    }

    private void FadeToGame() {
        ShowMainMenu(false);
        ShowGameUI(true);
        // afterFade = null;
        transitionController.FadeIn();
    }

    private void OnFadeComplete() {
        afterFade.Invoke();
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