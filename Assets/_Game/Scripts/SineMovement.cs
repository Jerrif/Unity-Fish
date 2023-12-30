using UnityEngine;

public class SineMovement : MonoBehaviour {
    // TODO: could add a small random freq/amp offset for each fish
    [SerializeField] private float amplitude = 1f;
    [SerializeField] private float frequency = 1f;
    private Vector2 startPos;
    private float randOffset;

    void OnEnable() {
        startPos = transform.position;
        randOffset = Random.Range(-Mathf.PI/2, Mathf.PI/2);
        Vector2 nextPos = transform.position;
        nextPos.y = Mathf.Sin(Time.time * frequency + randOffset) * amplitude + startPos.y;
        transform.position = nextPos;
    }

    void Update() {
        Vector2 nextPos = transform.position;
        nextPos.y = Mathf.Sin(Time.time * frequency + randOffset) * amplitude + startPos.y;
        transform.position = nextPos;
    }
}
