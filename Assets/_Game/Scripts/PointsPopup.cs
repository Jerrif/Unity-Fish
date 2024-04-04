using UnityEngine;

public class PointsPopup : MonoBehaviour {
    [SerializeField] private float speed = 0.5f;
    [SerializeField] private float lifetime = 1.5f;
    [SerializeField] private float fadeTime = 0.75f;
    private TextFader textFader;
    private float elapsed = 0f;

    private void Awake() {
        textFader = GetComponent<TextFader>();
    }

    private void OnEnable() {
        elapsed = 0f;
    }

    private void Update() {
        if (elapsed >= lifetime) {
            gameObject.SetActive(false);
            return;
        }
        if (elapsed >= lifetime - fadeTime) {
            StartCoroutine(textFader.StartFade(fadeTime));
        }
        // TODO: give this a shaping function (smoothstep?)
        transform.position += Vector3.up * speed * Time.deltaTime;
        elapsed += Time.deltaTime;
    }
}
