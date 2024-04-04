using System.Collections;
using TMPro;
using UnityEngine;

// REFACTOR: combine this with the `SpriteFader` script

public class TextFader : MonoBehaviour {
    private TextMeshPro tr;

    private void Awake() {
        tr = GetComponent<TextMeshPro>();
    }

    private void OnEnable() {
        tr.color = new Color(1f, 1f, 1f, 1f);
    }

    public IEnumerator StartFade(float duration) {
        Color newColor = tr.color;
        float progress = 0f;
        while (progress < duration) {
            progress += Time.deltaTime;
            newColor.a = Mathf.SmoothStep(1f, 0f, progress / duration);
            tr.color = newColor;
            yield return null;
        }
    }
}
