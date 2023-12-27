using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField] private HookController hookController;
    [SerializeField] private FishSpawner[] fishSpawners;

    void OnEnable() {
        if (hookController == null || fishSpawners.Length == 0) {
            Debug.LogError("woah gotta hook up the hook controller & fish spawners to the game manager");
            return;
        }
        // "hook" up the HookLanded event from hookController to a function in here
        hookController.HookLanded += testEvent;
    }

    void OnDisable() {
        // gotta unsubscribe
        hookController.HookLanded -= testEvent;
    }

    void Start() {
    }

    void testEvent() {
        print("yes game maganer got the evnt");
    }

    void Update() {
        
    }
}
