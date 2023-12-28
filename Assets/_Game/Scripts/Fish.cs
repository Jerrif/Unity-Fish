using System;
using UnityEngine;

public class Fish : MonoBehaviour {
    [SerializeField] private float speed = 1f;
    [SerializeField] private bool showHitbox = false;
    private float distanceToTravel = 15f;

    private Vector2 startPos;
    public SpriteRenderer sprite { get; private set; }
    public event Action<Fish> died;

    private void Start() {
        startPos = transform.position;
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        transform.position += Vector3.left * Time.deltaTime * speed;
        if (distanceTravelled() >= distanceToTravel) {
            DiedOfNaturalCauses();
            Destroy(gameObject);
        }
    }

    public float distanceTravelled() {
        return Mathf.Abs(transform.position.x - startPos.x);
    }

    private void DiedOfNaturalCauses() {
        died?.Invoke(this);
    }

    public void Caught() {
        Destroy(gameObject);
    }

    private void OnDrawGizmos() {
        if (!showHitbox) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, sprite.bounds.size);
    }
}
