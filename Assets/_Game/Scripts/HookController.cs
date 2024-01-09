using System;
using UnityEngine;

public class HookController : MonoBehaviour {
    
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private GameObject hookConstraints;
    [SerializeField] public float hookCastingTime = 3f;
    public float hookCastingTimeElapsed { get; private set; } = 0f;

    public SpriteRenderer sprite { get; private set; }
    private Bounds _constraintsArea;

    private bool casting = false;
    // public bool casting { get; private set; } = false;

    public event Action<float> HookCast;
    public event Action HookLanded;

    private void Awake() {
       if (!hookConstraints) {
            print("Yo hook constraints not set in HookController");
       }
        sprite = GetComponent<SpriteRenderer>();
        _constraintsArea = hookConstraints.GetComponent<SpriteRenderer>().bounds;
    }

    private void Update() {
        if (casting) {
            if (hookCastingTimeElapsed < hookCastingTime) {
                hookCastingTimeElapsed += Time.deltaTime;
                return;
            }
            hookCastingTimeElapsed = 0f;
            casting = false;
            
            // emit the HookLanded event, GameManager subs to this and does the collision checks
            HookLanded?.Invoke();
        }

        HandleMovement();

        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space)) {
            CastHook();
        }
    }

    private void CastHook() {
        casting = true;
        HookCast?.Invoke(hookCastingTime);
    }

    private void HandleMovement() {
        Vector2 nextPos = (Vector2)transform.position;
        nextPos += new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * moveSpeed * Time.deltaTime;
        nextPos = KeepInBounds(nextPos);
        transform.position = nextPos;
    }

    private Vector2 KeepInBounds(Vector2 pos) {
        Bounds hookArea = sprite.bounds;
        if (hookArea.min.x < _constraintsArea.min.x) {
            pos.x = _constraintsArea.min.x + hookArea.extents.x;
        } else if (hookArea.max.x > _constraintsArea.max.x) {
            pos.x = _constraintsArea.max.x - hookArea.extents.x;
        }

        if (hookArea.min.y < _constraintsArea.min.y) {
            pos.y = _constraintsArea.min.y + hookArea.extents.y;
        } else if (hookArea.max.y > _constraintsArea.max.y) {
            pos.y = _constraintsArea.max.y - hookArea.extents.y;
        }

        return pos;
    }
}
