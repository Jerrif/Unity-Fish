using System;
using System.Collections;
using UnityEngine;

public class HookController : MonoBehaviour {
    
    [SerializeField] private float speed = 3f;
    [SerializeField] private GameObject hookConstraints;
    [SerializeField] private float hookCastingTime = 3f;
    private float hookCastingTimeElapsed = 0f;

    private Bounds _hookArea;
    private Bounds _constraintsArea;

    private bool casting = false;

    public event Action HookLanded;

    void Awake() {
       if (!hookConstraints) {
            print("Yo hook constraints not set in WASD");
       }
        _hookArea = GetComponent<SpriteRenderer>().bounds;
        _constraintsArea = hookConstraints.GetComponent<SpriteRenderer>().bounds;
    }

    void Update() {
        if (casting) {
            if (hookCastingTimeElapsed < hookCastingTime) {
                hookCastingTimeElapsed += Time.deltaTime;
                return;
            }
            hookCastingTimeElapsed = 0f;
            casting = false;
            
            // emit the HookLanded event, GameManager subscribes to this
            // and does the collision calculations over there
            HookLanded?.Invoke();
        }

        HandleMovement();

        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space)) {
            CastHook();
        }
    }

    private void CastHook() {
        print("Casting Hook");
        casting = true;
    }

    private void HandleMovement() {
        Vector2 nextPos = (Vector2)transform.position;
        nextPos += new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * speed * Time.deltaTime;
        nextPos = KeepInBounds(nextPos);
        transform.position = nextPos;
        
        // transform.Translate(move * speed * Time.deltaTime);
    }

    private Vector2 KeepInBounds(Vector2 pos) {
        if (pos.x - _hookArea.extents.x < _constraintsArea.center.x - _constraintsArea.extents.x) {
            pos.x = _constraintsArea.center.x - _constraintsArea.center.x - _constraintsArea.extents.x + _hookArea.extents.x;
        } else if (pos.x + _hookArea.extents.x > _constraintsArea.center.x + _constraintsArea.extents.x) {
            pos.x = _constraintsArea.center.x + _constraintsArea.extents.x - _hookArea.extents.x;
        }

        if (pos.y - _hookArea.extents.y < _constraintsArea.center.y - _constraintsArea.extents.y) {
            pos.y = _constraintsArea.center.y - _constraintsArea.center.y - _constraintsArea.extents.y + _hookArea.extents.y;
        } else if (pos.y + _hookArea.extents.y > _constraintsArea.center.y + _constraintsArea.extents.y) {
            pos.y = _constraintsArea.center.y + _constraintsArea.extents.y - _hookArea.extents.y;
        }
        return pos;
    }
}
