using UnityEngine;
using TMPro;

// Trying something a bit different for this UI - explanation in `UISystem.cs -> ShowGameOverUI()`
public class GameOverUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI catchesIntText;
    [SerializeField] private TextMeshProUGUI missesIntText;

    private void Start() {
        catchesIntText.SetText("138");
        missesIntText.SetText("10");
    }

    private void Update() {
        
    }
    
    public void SetCatches(int catches) {
        catchesIntText.SetText(catches.ToString());
    }

    public void SetMisses(int misses) {
        missesIntText.SetText(misses.ToString());
    }
}
