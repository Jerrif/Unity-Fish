using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISystem : Singleton<UISystem> {
    [SerializeField] private GameObject mainMenuUI;
    [SerializeField] private Button playButton; // should probably not hardcode this? or just walk the mainMenuUI tree to find it
    [SerializeField] private GameObject gameUI;
    [SerializeField] private TMP_Text gameCountdownText;
    // [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameOverUI gameOverUI;
    private ScreenTransition transitionController;

    protected override void Awake() {
        base.Awake();
        transitionController = GetComponent<ScreenTransition>();
    }

    private void Update() {
        if (GameManager.Instance.state == GameState.GAME_RUNNING) {
            gameCountdownText.SetText(GameManager.Instance.GetSecondsRemaining());
        }
    }

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

    public void InitAndShowGameUI() {
        ShowGameUI(true);
        ScoreManager.Instance.enabled = true;
        gameCountdownText.SetText(GameManager.Instance.gameLength.ToString());
    }

    public void PlayButtonPressed() {
        playButton.enabled = false;
        GameManager.Instance.UpdateGameState(GameState.GAME_START);
    }

    public void MainMenuButtonPressed() {
        GameManager.Instance.UpdateGameState(GameState.MAIN_MENU);
    }

    public void ShowGameUI(bool active) {
        gameUI.SetActive(active);
    }

    public void ShowMainMenu(bool active) {
        playButton.enabled = true;
        mainMenuUI.SetActive(active);
    }

    public void ShowGameOverUI(bool active) {
        // gameOverUI.SetActive(active);
        // note: I'm intentionally doing this in a different way than the other 2 UIs for now.
        // I attached the game-over logic to a separate script, which is attached to the `GameOverUI` root object
        // that's why this looks a little different, though I think this way is much better.
        gameOverUI.gameObject.SetActive(active);
        gameOverUI.SetCatches(ScoreManager.Instance.GetCatches());
        gameOverUI.SetMisses(ScoreManager.Instance.GetMisses());
    }

    public void UnloadAllUI() {
        mainMenuUI.SetActive(false);
        gameUI.SetActive(false);
        // gameOverUI.SetActive(false);
        gameOverUI.gameObject.SetActive(false);
    }
}