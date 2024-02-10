using UnityEngine;
using System;

public class FishSpawner : MonoBehaviour {
    [SerializeField] private Vector2 spawnAreaSize;
    [SerializeField] private Fish[] fishPrefabs;
    [SerializeField] private int spawnsPerWave = 4;
    [SerializeField] private float secondsBetweenSpawns = 3.5f;
    [SerializeField] private float randomSpawnVariation = 1.0f;
    [SerializeField] private DIRECTION direction = DIRECTION.RIGHT;
    private float spawnTimer = 0f;

    public event Action<Fish> spawned;

    private void Start() {
        // TODO: I think this will be moved to a (non-unity) function, to be called by the `GameManager`
        SpawnFishGroup(spawnsPerWave);
    }

    private void Update() {
        if (spawnTimer < secondsBetweenSpawns) {
            spawnTimer += Time.deltaTime;
        } else {
            spawnTimer = 0f + UnityEngine.Random.Range(-randomSpawnVariation, randomSpawnVariation);
            SpawnFishGroup(spawnsPerWave);
        }
    }

    private void SpawnFishGroup(int count) {
        Vector2 pos = transform.position;
        Vector2 spawnPoint = new Vector2(
                UnityEngine.Random.Range(pos.x - spawnAreaSize.x/2, pos.x + spawnAreaSize.x/2),
                UnityEngine.Random.Range(pos.y - spawnAreaSize.y/2, pos.y + spawnAreaSize.y/2));
        for (int i=0; i < count; i++) {
            float variation = UnityEngine.Random.Range(-1f, 1f);
            SpawnFish(new Vector2(spawnPoint.x + variation, spawnPoint.y + variation));
        }
    }

    private void SpawnFish(Vector2 spawnPoint) {
        var fishToSpawn = fishPrefabs[UnityEngine.Random.Range(0, fishPrefabs.Length)];
        Fish newFish = Instantiate(fishToSpawn, spawnPoint, Quaternion.identity);

        newFish.setDirection(direction);

        spawned?.Invoke(newFish);
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, spawnAreaSize);
    }
}
