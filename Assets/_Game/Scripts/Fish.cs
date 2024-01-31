using System;
using UnityEngine;

public class Fish : MonoBehaviour {
    [SerializeField] private float speed = 1f;
    [SerializeField] private bool showHitbox = false;
    private float distanceToTravel = 15f;

    private Vector2 startPos;
    private int moveDirection = 1;
    public SpriteRenderer sprite { get; private set; }
    private SpriteFader spriteFader;
    private bool markedForDeath = false;
    public event Action<Fish> diedOfNaturalCauses;

    private void Awake() {
        sprite = GetComponent<SpriteRenderer>();
        spriteFader = gameObject.AddComponent<SpriteFader>();
    }

    private void Start() {
        startPos = transform.position;
        spriteFader.StartFade(FADE_DIRECTION.IN, 2f);
    }

    private void Update() {
        transform.position += Vector3.left * moveDirection * Time.deltaTime * speed;
        if (!markedForDeath && distanceTravelled() >= distanceToTravel) {
            MarkForDeath();
        }
    }

    public void setDirection(DIRECTION direction) {
        /// -1 is right, 1 is left
        moveDirection = (int)direction;
        transform.rotation = Quaternion.Euler(0f, 0f, 45f * moveDirection);
        sprite.flipX = moveDirection == 1;
    }

    public float distanceTravelled() {
        return Mathf.Abs(transform.position.x - startPos.x);
    }

    private void MarkForDeath() {
        markedForDeath = true;
        spriteFader.StartFade(FADE_DIRECTION.OUT, 2f);
        spriteFader.fadeComplete += DiedOfNaturalCauses;
    }

    private void DiedOfNaturalCauses() {
        spriteFader.fadeComplete -= DiedOfNaturalCauses;
        diedOfNaturalCauses?.Invoke(this);
        Destroy(gameObject);
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
