using System;
using UnityEngine;

public class FadeSprite : MonoBehaviour {
    public float fadeDuration { get; private set; } = 1f;
    private float progress = 0f;
    private bool fading = false;

    private float desiredAlpha;
    private float startAlpha;
    private Color newColor;
    private SpriteRenderer sprite;

    public event Action fadeComplete;

    private void OnEnable() {
        sprite = GetComponent<SpriteRenderer>();
        newColor = new Color(1f, 1f, 1f, 1f);
    }

    private void Update() {
        if (fading) {
            Fade();
        }
    }

    // TODO: could just modify this to be like
    // StartFade(float from, float to, float fadeDuration)
    // so you're not locked to 0 or 100% alpha
    public void StartFade(Direction direction) {
        desiredAlpha = (float)direction;
        startAlpha = ((int)direction) ^ 1;
        fading = true;
        progress = 0f;
        newColor.a = startAlpha;
        sprite.color = newColor;
    }

    private void Fade() {
        if (progress >= fadeDuration) {
            fading = false;
            progress = 0f;
            fadeComplete?.Invoke();
            return;
        }

        // newColor.a = Mathf.MoveTowards(newColor.a, desiredAlpha, Time.deltaTime / fadeDuration);
        newColor.a = Mathf.SmoothStep(startAlpha, desiredAlpha, progress / fadeDuration);
        sprite.color = newColor;
        progress += Time.deltaTime;
    }

    public enum Direction : ushort {
        IN = 1,
        OUT = 0
    }
}
