using UnityEngine;

public class ScreenTransition : MonoBehaviour {
    [SerializeField] private Material transitionMaterial;
    [SerializeField] private float transitionSpeed = 1f;
    public float fadeTarget { get; private set; } = 0f;
    public float fadeCurrent { get; private set; } = 0f;

    // public event Action fadeOutComplete;
    // public event Action fadeInComplete;

    private void OnEnable() {
        fadeTarget = 0f;
        fadeCurrent = 0f;
    }

    private void Update() {
        // TODO: delete these, obviously
        if (Input.GetKeyDown(KeyCode.Z)) FadeOut();
        if (Input.GetKeyDown(KeyCode.X)) FadeIn();

        if (fadeCurrent < fadeTarget) {
            fadeCurrent += transitionSpeed * Time.deltaTime;
            if (fadeCurrent > fadeTarget) {
                fadeCurrent = fadeTarget;
                // fadeOutComplete?.Invoke();
            }
            transitionMaterial.SetFloat("_Cutoff", fadeCurrent);
        } else if (fadeCurrent > fadeTarget) {
            fadeCurrent -= transitionSpeed * Time.deltaTime;
            if (fadeCurrent < 0f) {
                fadeCurrent = 0f;
                // fadeInComplete?.Invoke();
            }
            transitionMaterial.SetFloat("_Cutoff", fadeCurrent);
        }
    }

    public void FadeIn() {
        fadeTarget = 0f;
    }

    public void FadeOut() {
        fadeTarget = 1f;
    }
}
