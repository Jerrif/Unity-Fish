using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISystem : Singleton<UISystem> {
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private Button playButton; // should probably not hardcode this? or just walk the mainMenuUI tree to find it
    [SerializeField] private GameObject gameUI;
    [SerializeField] private TMP_Text gameCountdownText;
    private ScreenTransition transitionController;

    protected override void Awake() {
        base.Awake();
        transitionController = GetComponent<ScreenTransition>();
    }

    private void OnEnable() {
        GameManager.gameStateChanged += OnGameStateChanged;
    }

    private void OnDestroy() {
        GameManager.gameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newState) {
    //     // if (newState == GameState.MAIN_MENU) {
    //     //     Instantiate(mainMenuPanel);
    //     // }
    //     mainMenuPanel.SetActive(newState == GameState.MAIN_MENU);
    }

    private void Update() {
        if (GameManager.Instance.state == GameState.GAME_RUNNING) {
            gameCountdownText.SetText(GameManager.Instance.GetSecondsRemaining());
        }
    }

    // public IEnumerator FadeOutCoroutine() {
    //     transitionController.FadeOut();
    //     while (transitionController.fadeCurrent != transitionController.fadeTarget) {
    //         yield return false;
    //     }
    //     yield return true;
    // }

    public bool FadeOut() {
        transitionController.FadeOut();
        while (transitionController.fadeCurrent != transitionController.fadeTarget) {
            return false;
        }
        return true;
    }

    public bool FadeIn() {
        transitionController.FadeIn();
        while (transitionController.fadeCurrent != transitionController.fadeTarget) {
            return false;
        }
        return true;
    }

    public void InitGameUI() {
        ShowGameUI(true);
        ScoreManager.Instance.enabled = true;
        gameCountdownText.SetText(GameManager.Instance.gameLength.ToString());
    }

    // public IEnumerator FadeToGame() {
    //     transitionController.FadeOut();
    //     while (transitionController.fadeCurrent != transitionController.fadeTarget) {
    //         yield return null;
    //     }
    //     playButton.enabled = true;
    //     ShowGameUI(true);
    //     ShowMainMenu(false);
    //     ScoreManager.Instance.Init(); // annoying hack
    //     gameCountdownText.SetText(GameManager.Instance.gameLength.ToString()); // annoying hack
    //     yield return new WaitForSecondsRealtime(0.5f);
    //     transitionController.FadeIn();
    //     while (transitionController.fadeCurrent != transitionController.fadeTarget) {
    //         yield return null;
    //     }
        
    //     GameManager.Instance.UpdateGameState(GameState.GAME_START);
    // }

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
        GameManager.Instance.UpdateGameState(GameState.GAME_START);
        // StartCoroutine(FadeToGame());
        // StartCoroutine(FadeOutCoroutine());
    }

    public void ShowGameUI(bool active) {
        gameUI.SetActive(active);
    }

    public void ShowMainMenu(bool active) {
        mainMenuUI.SetActive(active);
    }
}