// TODO: refactor this to have an empty parent object (transform) to act as the "HookController", then the `HookBox`, `Hook`, and `Splash`
// can all be child objects (without needing logic scripts?)
// check this vid for advice: https://www.youtube.com/watch?v=ts24UWC0mY4

using System;
using UnityEngine;

public class HookController : MonoBehaviour {
    
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private GameObject hookConstraints;
    [SerializeField] public float hookCastingTime = 3f;
    [SerializeField] public float hookReelingTime = 1f; // delay period after landing, before being able to cast again
    public float hookCastingTimeElapsed { get; private set; } = 0f;
    public float hookReelingTimeElapsed { get; private set; } = 0f;

    public SpriteRenderer sprite { get; private set; }
    private Bounds _constraintsArea;

    public bool casting { get; private set; } = false;
    public bool reeling { get; private set; } = false;

    public event Action<float> HookCast;
    public event Action HookLanded;
    // public event Action ReelingFinished;

    private void Awake() {
       if (!hookConstraints) {
            print("Yo hook constraints not set in HookController");
       }
        sprite = GetComponent<SpriteRenderer>();
        _constraintsArea = hookConstraints.GetComponent<SpriteRenderer>().bounds;
    }

    private void OnEnable() {
        hookCastingTimeElapsed = 0f;
        hookReelingTimeElapsed = 0f;
        casting = false;
        reeling = false;
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

            ReelIn();
        }

        if (reeling) {
            if (hookReelingTimeElapsed < hookReelingTime) {
                hookReelingTimeElapsed += Time.deltaTime;
                return;
            }
            hookReelingTimeElapsed = 0f;
            reeling = false;

            // ReelingFinished?.Invoke();
        }

        HandleMovement();

        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space)) {
            CastHook();
        }
    }

    private void ReelIn() {
        reeling = true;
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
        if (pos.x - hookArea.extents.x < _constraintsArea.min.x) {
            pos.x = _constraintsArea.min.x + hookArea.extents.x;
        } else if (pos.x + hookArea.extents.x > _constraintsArea.max.x) {
            pos.x = _constraintsArea.max.x - hookArea.extents.x;
        }

        if (pos.y - hookArea.extents.y < _constraintsArea.min.y) {
            pos.y = _constraintsArea.min.y + hookArea.extents.y;
        } else if (pos.y + hookArea.extents.y > _constraintsArea.max.y) {
            pos.y = _constraintsArea.max.y - hookArea.extents.y;
        }
        return pos;
    }
}
