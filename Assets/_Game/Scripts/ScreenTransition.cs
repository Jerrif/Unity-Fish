using UnityEngine;

// TODO: Maybe I can simplify this and combine `SimpleBlit.cs` into here?
// Or at least set the material in `SimpleBlit` from here. Or something.
public class ScreenTransition : MonoBehaviour {
    [SerializeField] private Material transitionMaterial;
    [SerializeField] private float transitionSpeed = 1f;
    private float fadeTarget = 0f;
    private float fadeCurrent = 0f;
    private bool fading = false;

    private void OnEnable() {
        fadeTarget = 0f;
        fadeCurrent = 0f;
        fading = false;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Z)) FadeOut();
        if (Input.GetKeyDown(KeyCode.X)) FadeIn();

        // if (!fading) return;
        // if (fadeCurrent < fadeTarget) {
        //     // currentFade_t += Time.deltaTime;
        //     fadeCurrent = Mathf.MoveTowards(fadeCurrent, fadeTarget, transitionSpeed * Time.deltaTime);
        // }

        if (fadeCurrent < fadeTarget) {
            fadeCurrent += transitionSpeed * Time.deltaTime;
            if (fadeCurrent > fadeTarget) fadeCurrent = fadeTarget;
            transitionMaterial.SetFloat("_Cutoff", fadeCurrent);
        } else if (fadeCurrent > fadeTarget) {
            fadeCurrent -= transitionSpeed * Time.deltaTime;
            if (fadeCurrent < 0f) fadeCurrent = 0f;
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
