using UnityEngine;

public class SineMovement : MonoBehaviour {
    // TODO: could add a small random freq/amp offset for each fish
    [SerializeField] private float amplitude = 1f;
    [SerializeField] private float frequency = 1f;
    private Vector2 startPos;
    private float startOffset;

    void Start() {
        startPos = transform.position;
        startOffset = Random.Range(-Mathf.PI/2, Mathf.PI/2);
        amplitude += Random.Range(amplitude * -0.15f, amplitude * 0.15f);
        frequency += Random.Range(frequency * -0.15f, frequency * 0.15f);
        Vector2 nextPos = transform.position;
        nextPos.y = Mathf.Sin(Time.time * frequency + startOffset) * amplitude + startPos.y;
        transform.position = nextPos;
    }

    void Update() {
        Vector2 nextPos = transform.position;
        nextPos.y = Mathf.Sin(Time.time * frequency + startOffset) * amplitude + startPos.y;
        transform.position = nextPos;
    }
}
