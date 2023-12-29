using UnityEngine;

public class FadeSprite : MonoBehaviour {
    [SerializeField] private float fadeDuration = 2f;
    private float progress = 0f;
    private bool fading = false;

    [Header("Fade in / out?")]
    [SerializeField] private bool fadeIn = true;
    // public Direction direction = Direction.IN;
    private float desiredAlpha; // can't cast bools to int in C#
    private float startAlpha;
    private Color newColor;
    private SpriteRenderer sprite;

    private void Start() {
        desiredAlpha = fadeIn ? 1f : 0f;
        startAlpha = fadeIn ? 0f : 1f;
        sprite = GetComponent<SpriteRenderer>();
        newColor = new Color(1f, 1f, 1f, startAlpha);
        sprite.color = newColor;
        // StartFade();
    }

    private void Update() {
        if (fading) {
            Fade();
        }
    }

    public void StartFade() {
        fading = true;
        progress = 0f;
    }

    private void Fade() {
        if (progress >= fadeDuration) {
            fading = false;
            progress = 0f;
            return;
        }
        progress += Time.deltaTime;

        // newColor.a = Mathf.MoveTowards(newColor.a, desiredAlpha, Time.deltaTime / fadeDuration);
        newColor.a = Mathf.SmoothStep(startAlpha, desiredAlpha, progress / fadeDuration);
        sprite.color = newColor;
    }

    public enum Direction : ushort {
        IN = 0,
        OUT = 1
    }
}
