using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour {

    [SerializeField] private Vector2 spawnAreaSize;
    [SerializeField] private Fish[] fishPrefabs;
    // [SerializeField] private int spawnsPerWave = 3;
    [SerializeField] private float secondsBetweenSpawns = 1f;

    private List<Fish> spawnedFish;

    private float xMin;
    private float xMax;
    private float yMin;
    private float yMax;

    // `yield return new` requires the `IEnumerator` return type
    private IEnumerator Start() {
        spawnedFish = new List<Fish>();

        xMin = transform.position.x - spawnAreaSize.x / 2;
        xMax = transform.position.x + spawnAreaSize.x / 2;
        yMin = transform.position.y - spawnAreaSize.y / 2;
        yMax = transform.position.y + spawnAreaSize.y / 2;

        for (int i = 0; i <= 5; i++) {
            SpawnFish();
            yield return new WaitForSeconds(0.1f);
        }

        InvokeRepeating(nameof(SpawnFish), secondsBetweenSpawns, secondsBetweenSpawns);
    }

    void Update() {
    }

    private void SpawnFish() {
        var fishToSpawn = fishPrefabs[Random.Range(0, fishPrefabs.Length)];
        Vector2 point = new Vector2(Random.Range(xMin, xMax), Random.Range(yMin, yMax));
        Fish newFish = Instantiate(fishToSpawn, point, Quaternion.identity);
        spawnedFish.Add(newFish);
        newFish.died += FishDied;
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, spawnAreaSize);
    }

    private void FishDied(Fish dedFish) {
        print("fish died event in fish spawner");
        print("TODO: do something, remove from array etc" + dedFish);
    }
}
