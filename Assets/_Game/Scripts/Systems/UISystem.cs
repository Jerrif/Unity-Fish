using UnityEngine;

public class UISystem : MonoBehaviour {
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private ClickyButton button;

    private void OnEnable() {
        GameManager.gameStateChanged += OnGameStateChanged;
    }

    private void OnDestroy() {
        GameManager.gameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newState) {
        print("waow new state dood! " + newState.ToString());
    }

    private void Start() {
        
    }

    private void Update() {
        
    }


}

        
// Game Manager - Controlling the flow of your game [Unity Tutorial]
// https://www.youtube.com/watch?v=4I0vonyqMi8&t=79s
// https://www.youtube.com/watch?v=zJOxWmVveXU
// https://www.youtube.com/watch?v=tE1qH8OxO2Y&t=356s