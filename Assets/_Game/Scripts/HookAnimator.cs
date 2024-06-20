// I could have used events to do all this (HookController emits "casting" event w/ duration parameter + "landed" event)
// and this script would then subscribe to those events.
// However, in this instance, the GameObject that HookAnimator is attached to is a (Unity)child of the HookController
// so I wanted to try something "simpler/more straight forward" (?).
// So I just access the public HookController variables directly from here. I think that's fine
// LMAO, I decided to just do it with events instead

using UnityEngine;

public class HookAnimator : MonoBehaviour {
    [SerializeField] private float yOffset = 2.5f;

    private HookController _hookController;
    private SpriteRenderer _sprite;
    private SpriteFader spriteFader;
    [SerializeField] private Animator splashAnimator;

    private void Awake() {
        _hookController = GetComponentInParent<HookController>();
        _sprite = GetComponent<SpriteRenderer>();
        spriteFader = gameObject.AddComponent<SpriteFader>();
        if (splashAnimator == null) {
            print("woah no splahs animtora");
        }
        splashAnimator.keepAnimatorStateOnDisable = true;
    }

    private void OnEnable() {
        _hookController.HookCastEvent += OnHookCast;
        _hookController.HookLandedEvent += OnHookLanded;
        Color c = new Color(1f, 1f, 1f, 0f); // set alpha to 0 at start
        _sprite.color = c;
        transform.position = new Vector2(transform.position.x, transform.position.y + yOffset);
    }

    private void Update() {
        if (_hookController.casting) {
            float nextY = Mathf.SmoothStep(_hookController.transform.position.y + yOffset, _hookController.transform.position.y, _hookController.hookCastingTimeElapsed / _hookController.hookCastingTime);
            transform.position = new Vector2(transform.position.x, nextY);
            return;
        }

        if (_hookController.reeling) {
            float nextY = Mathf.Lerp(_hookController.transform.position.y, _hookController.transform.position.y + yOffset, _hookController.hookReelingTimeElapsed / _hookController.hookReelingTime);
            transform.position = new Vector2(transform.position.x, nextY);
            return;
        }
    }

    private void OnHookCast(float hookCastingTime) {
        spriteFader.StartFade(FADE_DIRECTION.IN, hookCastingTime / 3f);
    }

    private void OnHookLanded() {
        spriteFader.StartFade(FADE_DIRECTION.OUT, _hookController.hookReelingTime / 1.5f);
        splashAnimator.SetTrigger("HookLandedAnimTrigger");
    }

    private void OnDisable() {
        _hookController.HookCastEvent -= OnHookCast;
        _hookController.HookLandedEvent -= OnHookLanded;
        splashAnimator.Play("Idle", 0, 0f);
    }
}
