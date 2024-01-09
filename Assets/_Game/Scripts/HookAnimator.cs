// I could have used events to do all this (HookController emits "casting" event w/ duration parameter + "landed" event)
// and this script would then subscribe to those events.
// However, in this instance, the GameObject that HookAnimator is attached to is a (Unity)child of the HookController
// so I wanted to try something "simpler/more straight forward" (?).
// So I just access the public HookController variables directly from here. I think that's fine
// LMAO, I decided to just do it with events instead

using UnityEngine;

public class HookAnimator : MonoBehaviour {
    private HookController _hookController;
    private SpriteRenderer _sprite;
    private bool casting = false;

    void Awake() {
        _hookController = GetComponentInParent<HookController>();
        _sprite = GetComponent<SpriteRenderer>();
        _hookController.HookCast += OnHookCast;
        _hookController.HookLanded += OnHookLanded;
    }

    void Update() {
    }

    void OnHookCast(float hookCastingTime) {
        _sprite.flipX = true;
        // hmm doesn't work like that, still have to put this in update, under a "casting" flag (similar to hookcontroller)
        transform.position = Vector2.MoveTowards(transform.position, _hookController.transform.position, 1.1f);
    }

    void OnHookLanded() {
        _sprite.flipX = false;
    }

    private void OnDisable() {
        _hookController.HookCast -= OnHookCast;
        _hookController.HookLanded -= OnHookLanded;
    }
}
