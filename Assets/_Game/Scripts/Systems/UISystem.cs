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

    // TODO: Use coroutines?
    // https://gamedevbeginner.com/coroutines-in-unity-when-and-how-to-use-them/
    // https://www.youtube.com/watch?v=t4m8pmahbzY
    // https://www.youtube.com/watch?v=7RBI9mb8s3E
    // maybe combine it with Actions like in the last video


    private IEnumerator LoadGameUI() {
        transitionController.FadeOut();
        while (transitionController.fadeCurrent != transitionController.fadeTarget) {
            yield return null;
        }
        playButton.enabled = true;
        ShowMainMenu(false);
        ShowGameUI(true);
        yield return new WaitForSeconds(0.5f);
        transitionController.FadeIn();
        while (transitionController.fadeCurrent != transitionController.fadeTarget) {
            yield return null;
        }
        // TODO: GameMangager.updatestate to new state
    }

    public void PlayButtonPressed() {
        playButton.enabled = false;
        StartCoroutine(LoadGameUI());
    }

    public void ShowGameUI(bool active) {
        gameUI.SetActive(active);
    }

    public void ShowMainMenu(bool active) {
        mainMenuUI.SetActive(active);
    }
}