using UnityEngine;

public class PointsTextObjectPool : MonoBehaviour {

    [SerializeField] private GameObject pointsTextPrefab;
    const int MAX_POOL_SIZE = 20;
    private GameObject[] pool;

    private void Start() {
        pool = new GameObject[MAX_POOL_SIZE];
        // pointsTextPrefab.SetActive(false);
        for (int i = 0; i < MAX_POOL_SIZE; i++) {
            pool[i] = Instantiate(pointsTextPrefab, transform.position, Quaternion.identity);
            pool[i].SetActive(false);
        }
    }

    public void Activate(Vector3 position) {
        for (int i = 0; i < MAX_POOL_SIZE; i++) {
            if (!pool[i].activeSelf) {
                pool[i].transform.position = position;
                pool[i].SetActive(true);
                break;
            }
        }
    }
}
