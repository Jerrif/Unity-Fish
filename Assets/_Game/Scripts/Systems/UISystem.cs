using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UISystem : Singleton<UISystem> {
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private Button playButton; // should probably not hardcode this? or just walk the mainMenuUI tree to find it
    [SerializeField] private GameObject gameUI;
    private ScreenTransition transitionController;

    protected override void Awake() {
        base.Awake();
        transitionController = GetComponent<ScreenTransition>();
    }

    private void OnEnable() {
        GameManager.gameStateChanged += OnGameStateChanged;
        // transitionController.fadeOutComplete += OnFadeOutComplete;
        // transitionController.fadeInComplete += OnFadeInComplete;
    }

    private void OnDestroy() {
        GameManager.gameStateChanged -= OnGameStateChanged;
        // transitionController.fadeOutComplete -= OnFadeOutComplete;
        // transitionController.fadeInComplete -= OnFadeInComplete;
    }

    private void OnGameStateChanged(GameState newState) {
    //     // if (newState == GameState.MAIN_MENU) {
    //     //     Instantiate(mainMenuPanel);
    //     // }
    //     mainMenuPanel.SetActive(newState == GameState.MAIN_MENU);
    }

    public IEnumerator FadeToGame() {
        transitionController.FadeOut();
        while (transitionController.fadeCurrent != transitionController.fadeTarget) {
            yield return null;
        }
        playButton.enabled = true;
        ShowGameUI(true);
        ShowMainMenu(false);
        ScoreManager.Instance.Init();
        yield return new WaitForSecondsRealtime(0.5f);
        transitionController.FadeIn();
        while (transitionController.fadeCurrent != transitionController.fadeTarget) {
            yield return null;
        }
        
        GameManager.Instance.UpdateGameState(GameState.GAME_START);
    }

    public IEnumerator FadeToMainMenu() {
        transitionController.FadeOut();
        while (transitionController.fadeCurrent != transitionController.fadeTarget) {
            yield return null;
        }
        ShowGameUI(false);
        ShowMainMenu(true);
        yield return new WaitForSecondsRealtime(0.5f);
        transitionController.FadeIn();
        while (transitionController.fadeCurrent != transitionController.fadeTarget) {
            yield return null;
        }
        
        playButton.enabled = true;
        GameManager.Instance.UpdateGameState(GameState.MAIN_MENU);
    }

    public void PlayButtonPressed() {
        playButton.enabled = false;
        StartCoroutine(FadeToGame());
    }

    public void ShowGameUI(bool active) {
        gameUI.SetActive(active);
    }

    public void ShowMainMenu(bool active) {
        mainMenuUI.SetActive(active);
    }
}