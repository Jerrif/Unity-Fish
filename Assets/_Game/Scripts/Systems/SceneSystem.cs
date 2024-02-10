using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSystem : MonoBehaviour {
    public static SceneSystem Instance;

    void Awake() {
        // standard Unity Singleton boilerplate
        if (Instance == null) {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }

        GameManager.gameStateChanged += OnGameStateChanged;
    }

    private void Start() {
    }

    private void OnDisable() {
        GameManager.gameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newState) {
        if (newState == GameState.MAIN_MENU) {
            LoadScene(0);
        }
    }

    public void LoadNextScene() {
        print("waoh loading next scene");
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        // TODO: Yo, I think this would be a perfect place for an `Assert`, wouldn't you agree?
        if (currentScene >= SceneManager.sceneCountInBuildSettings - 1) {
            Debug.LogWarning("Woah tried to load non-existent scene: " + (currentScene + 1));
            return;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadScene(int indexToLoad) {
        if (indexToLoad == SceneManager.GetActiveScene().buildIndex) {
            return;
        }
        SceneManager.LoadScene(indexToLoad);
    }

    /* watch these vids:
    https://www.youtube.com/watch?v=zObWVOv1GlE
    https://www.youtube.com/watch?v=4I0vonyqMi8&t=79s
    https://www.youtube.com/watch?v=OmobsXZSRKo // (watched already)
    https://gamedevbeginner.com/how-to-load-a-new-scene-in-unity-with-a-loading-screen/
    */

}
