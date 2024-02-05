using System;
using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    [SerializeField] private HookController hookController;
    [SerializeField] private FishSpawner[] fishSpawners;
    // TODO: hmm where should the responsibility be for making particle effects appear on caught fish?
    [SerializeField] private ParticleSystem particleManager;

    [SerializeField] float gameLength = 60f;
    [SerializeField] Timer timer;

    private List<Fish> aliveFish;

    // RESEARCH: this could be an event, or it could just have a reference to the scoreboard?
    // testing out a `static` event. The subscribing object (`ScoreManager`) doesn't need a reference to `GameManager` now.
    public static event Action<int> fishCaughtEvent;
    // this one's basically a long-ass signal relay: `Fish` -> `FishSpawner` -> `GameManager` (here) -> `ScoreManager`
    // maybe that's really dumb. Could just use a static event right on the Fish?
    public static event Action fishDiedOfNaturalCausesEvent;

    private void OnEnable() {
        if (hookController == null || fishSpawners.Length == 0) {
            Debug.LogError("woah gotta hook up the hook controller & fish spawners to the game manager");
            return;
        }
        // "hook" up (hehe) the HookLanded event from hookController to a function in here
        hookController.HookLanded += HookLanded;
    }

    private void OnDisable() {
        // gotta unsubscribe
        hookController.HookLanded -= HookLanded;
        foreach (FishSpawner fishSpawner in fishSpawners) {
            fishSpawner.spawned -= FishSpawned;
        }
    }

    private void Awake() {
        aliveFish = new List<Fish>();
        foreach (FishSpawner fishSpawner in fishSpawners) {
            fishSpawner.spawned += FishSpawned;
        }
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
        UnityEditor.EditorApplication.isPlaying = false;
    }

    private void HookLanded() {
        int numCaught = 0;
        Bounds hookBounds = hookController.sprite.bounds;
        List<Fish> caughtFish = new List<Fish>();

        foreach (Fish fish in aliveFish) {
            Bounds fishBounds = fish.sprite.bounds;
            if (fishBounds.Intersects(hookBounds)) {
                caughtFish.Add(fish);
                numCaught++;
            }
        }

        if (numCaught > 0) {
            foreach (Fish cf in caughtFish) {
                var emitParams = new ParticleSystem.EmitParams {
                    applyShapeToPosition = true,
                    position = cf.transform.position,
                };
                particleManager.Emit(emitParams, 40);
                aliveFish.Remove(cf);
                cf.Caught();
            }
            fishCaughtEvent?.Invoke(numCaught);
        }
    }

    private void FishSpawned(Fish newFish) {
        aliveFish.Add(newFish);
        newFish.diedOfNaturalCauses += FishDied;
    }

    private void FishDied(Fish dedFish) {
        aliveFish.Remove(dedFish);
        dedFish.diedOfNaturalCauses -= FishDied;
        fishDiedOfNaturalCausesEvent?.Invoke();
    }
}
