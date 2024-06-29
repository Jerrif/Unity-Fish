using System;
using UnityEngine;
using System.Collections.Generic;

public class FishManager : Singleton<FishManager> {
    [SerializeField] private HookController hookController;
    [SerializeField] private FishSpawner[] fishSpawners;
    // TODO: hmm where should the responsibility be for making particle effects appear on caught fish?
    [SerializeField] private ParticleSystem particleManager;
    [SerializeField] private PointsTextObjectPool pointsTextObjectPool;

    [SerializeField] private float swimmableAreaTop;
    [SerializeField] private float swimmableAreaBottom;

    private List<Fish> aliveFish;

    // RESEARCH: this could be an event, or it could just have a reference to the scoreboard?
    // testing out a `static` event. The subscribing object (`ScoreManager`) doesn't need a reference to `FishManager` now.
    public static event Action<int> fishCaughtEvent;
    // this one's basically a long-ass signal relay: `Fish` -> `FishSpawner` -> `FishManager` (here) -> `ScoreManager`
    // maybe that's really dumb. Could just use a static event right on the Fish?
    public static event Action fishDiedOfNaturalCausesEvent;

    private void OnEnable() {
        aliveFish = new List<Fish>();
        if (hookController == null || fishSpawners.Length == 0) {
            Debug.LogError("woah gotta hook up the hook controller & fish spawners to the game manager");
            return;
        }
        // "hook" up (hehe) the HookLanded event from hookController to a function in here
        // note: strange, changing the hook events to be `static` means I can't sub to the event from the actual reference
        // hookController.HookLandedEvent += OnHookLanded;
        HookController.HookLandedEvent += OnHookLanded;

        foreach (FishSpawner fishSpawner in fishSpawners) {
            fishSpawner.enabled = true;
            fishSpawner.spawned += FishSpawned;
            fishSpawner.SpawnFishGroup(fishSpawner.spawnsPerWave);
        }
    }

    private void OnDisable() {
        // gotta unsubscribe
        // hookController.HookLandedEvent -= OnHookLanded;
        HookController.HookLandedEvent -= OnHookLanded;
        foreach (FishSpawner fishSpawner in fishSpawners) {
            fishSpawner.spawned -= FishSpawned;
            fishSpawner.enabled = false;
        }

        foreach (Fish fish in aliveFish) {
            Destroy(fish.gameObject);
        }
    }

    private void OnHookLanded() {
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
        if (numCaught > 3) {
            // NOTE: this wouldn't be possible to do if the fish-caught logic was on the `Fish` itself.
            print("WOW that's a lot of fish! " + numCaught);
        }

        // TODO: why is this here and not in the intersection check?
        if (numCaught > 0) {
            foreach (Fish cf in caughtFish) {
                var emitParams = new ParticleSystem.EmitParams {
                    applyShapeToPosition = true,
                    position = cf.transform.position,
                };
                particleManager.Emit(emitParams, 40);
                pointsTextObjectPool.Activate(cf.transform.position);
                aliveFish.Remove(cf);
                cf.Caught();

                // SFXManager.Instance.Play(SFXType.CaughtFish);
            }
            fishCaughtEvent?.Invoke(numCaught);
        }
    }

    private void FishSpawned(Fish newFish) {
        aliveFish.Add(newFish);
        // newFish.setSwimmableArea(swimmableAreaTop, swimmableAreaBottom);
        if (newFish.TryGetComponent<SineMovement>(out SineMovement sineMovement)) {
            sineMovement.setSwimmableArea(swimmableAreaTop, swimmableAreaBottom);
        }
        newFish.diedOfNaturalCauses += FishDied;
    }

    private void FishDied(Fish dedFish) {
        aliveFish.Remove(dedFish);
        dedFish.diedOfNaturalCauses -= FishDied;
        fishDiedOfNaturalCausesEvent?.Invoke();
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.gray;
        Gizmos.DrawRay(new Vector3(-9, swimmableAreaTop, 0), Vector2.right * 18);
        Gizmos.DrawRay(new Vector3(-9, swimmableAreaBottom, 0), Vector2.right * 18);
    }
}
