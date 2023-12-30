using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    [SerializeField] private HookController hookController;
    [SerializeField] private FishSpawner[] fishSpawners;

    private List<Fish> aliveFish;

    private void OnEnable() {
        if (hookController == null || fishSpawners.Length == 0) {
            Debug.LogError("woah gotta hook up the hook controller & fish spawners to the game manager");
            return;
        }
        // "hook" up (hehe) the HookLanded event from hookController to a function in here
        hookController.HookLanded += HookLanded;

        foreach (FishSpawner fishSpawner in fishSpawners) {
            fishSpawner.spawned += FishSpawned;
        }
    }

    private void OnDisable() {
        // gotta unsubscribe
        hookController.HookLanded -= HookLanded;
        foreach (FishSpawner fishSpawner in fishSpawners) {
            fishSpawner.spawned -= FishSpawned;
        }
    }

    private void Start() {
        aliveFish = new List<Fish>();
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
                aliveFish.Remove(cf);
                cf.Caught();
            }
            print("Wow caught " + numCaught + " fish!");
        }
    }

    private void FishSpawned(Fish newFish) {
        aliveFish.Add(newFish);
        newFish.diedOfNaturalCauses += FishDied;
    }

    private void FishDied(Fish dedFish) {
        aliveFish.Remove(dedFish);
        dedFish.diedOfNaturalCauses -= FishDied;
    }
}
