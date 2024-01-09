// I could have used events to do all this (HookController emits "casting" event w/ duration parameter + "landed" event)
// and this script would then subscribe to those events.
// However, in this instance, the GameObject that HookAnimator is attached to is a (Unity)child of the HookController
// so I wanted to try something "simpler/more straight forward" (?).
// So I just access the public HookController variables directly from here. I think that's fine
// LMAO, I decided to just do it with events instead

using UnityEngine;

public class HookAnimatorEvents : MonoBehaviour {
    [SerializeField] private float yOffset = 2.5f;

    private HookController _hookController;
    private SpriteRenderer _sprite;
    private bool casting = false;
    private bool reeling = false;

    void Awake() {
        _hookController = GetComponentInParent<HookController>();
        _sprite = GetComponent<SpriteRenderer>();
        _hookController.HookCast += OnHookCast;
        _hookController.HookLanded += OnHookLanded;
        _hookController.ReelingFinished += OnReelingFinished;
    }

    void Update() {
        if (casting) {
            // transform.position = Vector2.MoveTowards(transform.position, _hookController.transform.position, 0.005f);
            float nextY = Mathf.Lerp(_hookController.transform.position.y + yOffset, _hookController.transform.position.y, _hookController.hookCastingTimeElapsed / _hookController.hookCastingTime);
            transform.position = new Vector2(transform.position.x, nextY);
            return;
        }

        if (reeling) {
            float nextY = Mathf.Lerp(_hookController.transform.position.y, _hookController.transform.position.y + yOffset, _hookController.hookReelingTimeElapsed / _hookController.hookReelingTime);
            transform.position = new Vector2(transform.position.x, nextY);
            return;
        }
    }

    void OnHookCast(float hookCastingTime) {
        _sprite.flipX = true;
        casting = true;
    }

    void OnHookLanded() {
        _sprite.flipX = false;
        casting = false;
        reeling = true;
    }

    void OnReelingFinished() {
        reeling = false;
    }

    private void OnDisable() {
        _hookController.HookCast -= OnHookCast;
        _hookController.HookLanded -= OnHookLanded;
        _hookController.ReelingFinished -= OnReelingFinished;
    }
}
