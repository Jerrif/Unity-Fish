using UnityEngine;
using System.Collections;
using System;

public class FishSpawner : MonoBehaviour {

    [SerializeField] private Vector2 spawnAreaSize;
    [SerializeField] private Fish[] fishPrefabs;
    // [SerializeField] private int spawnsPerWave = 3;
    [SerializeField] private float secondsBetweenSpawns = 1f;
    [SerializeField] private DIRECTION direction = DIRECTION.RIGHT;
    private float spawnTimer = 0f;

    public event Action<Fish> spawned;

    private float xMin;
    private float xMax;
    private float yMin;
    private float yMax;

    // `yield return new` requires the `IEnumerator` return type
    // private IEnumerator Start() {
    private void Start() {

        xMin = transform.position.x - spawnAreaSize.x / 2;
        xMax = transform.position.x + spawnAreaSize.x / 2;
        yMin = transform.position.y - spawnAreaSize.y / 2;
        yMax = transform.position.y + spawnAreaSize.y / 2;

        // for (int i = 0; i <= 5; i++) {
        //     SpawnFish();
        //     yield return new WaitForSeconds(0.1f);
        // }

        // for (int i = 0; i <= 5; i++) {
        //     SpawnFish();
        // }
        SpawnFishGroup(5);

        // BUG: with this method, the spawn rate can't be changed from the editor, while in play
        // InvokeRepeating(nameof(SpawnFish), secondsBetweenSpawns, secondsBetweenSpawns);

    }

    private void Update() {
        if (spawnTimer < secondsBetweenSpawns) {
            spawnTimer += Time.deltaTime;
        } else {
            spawnTimer = 0f;
            SpawnFishGroup(4);
        }
    }

    private void SpawnFishGroup(int count) {
        for (int i=0; i < count; i++) {
            SpawnFish();
        }
    }

    private void SpawnFish() {
        var fishToSpawn = fishPrefabs[UnityEngine.Random.Range(0, fishPrefabs.Length)];
        Vector2 point = new Vector2(UnityEngine.Random.Range(xMin, xMax), UnityEngine.Random.Range(yMin, yMax));
        Fish newFish = Instantiate(fishToSpawn, point, Quaternion.identity);

        // TODO: maybe make `DIRECTION` an enum? idk if it's reeeeeally necessary though
        newFish.setDirection(direction);

        spawned?.Invoke(newFish);
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, spawnAreaSize);
    }
}
