using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField] float gameLength = 60f;
    [SerializeField] Timer timer;

    private void Awake() {
        timer.timerExpired += TimeIsUp;
    }

    private void Start() {
        timer.StartTimer(gameLength);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }

    private void TimeIsUp() {
        print("waow time is up");
        SceneSystem.Instance.LoadScene(0);
    }

}
