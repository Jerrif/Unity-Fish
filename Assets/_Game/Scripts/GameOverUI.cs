using UnityEngine;
using TMPro;
using UnityEngine.UI;

// Trying something a bit different for this UI - explanation in `UISystem.cs -> ShowGameOverUI()`
public class GameOverUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI catchesIntText;
    [SerializeField] private TextMeshProUGUI missesIntText;
    [SerializeField] private Button playAgainButton;
    [SerializeField] private Button mainMenuButton;

    private void Awake() {
        catchesIntText.SetText("999");
        missesIntText.SetText("999");
    }

    private void Update() {
        
    }
    
    public void SetCatches(int catches) {
        catchesIntText.SetText(catches.ToString());
    }

    public void SetMisses(int misses) {
        missesIntText.SetText(misses.ToString());
    }

    private void OnEnable() {
        playAgainButton.interactable = true;
        mainMenuButton.interactable = true;
    }

    public void DisableButtons() {
        print("wow playing again");
        playAgainButton.interactable = false;
        mainMenuButton.interactable = false;
    }
}
