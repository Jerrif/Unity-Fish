using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BGMController : MonoBehaviour {
    private AudioSource audioSource;
    [SerializeField] private float maxVolume;
    [SerializeField] private float fadeDuration;
    [SerializeField] private AudioClip mainMenuBGM;
    [SerializeField] private AudioClip gameRunningBGM;
    [SerializeField] private AudioClip gameOverBGM;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = maxVolume;
    }

    private void OnEnable() {
        GameManager.gameStateChanged += OnGameStateChanged;
    }

    private void OnDisable() {
        GameManager.gameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newState) {
        AudioClip nextBGM;
        if (newState == GameState.MAIN_MENU) {
            nextBGM = mainMenuBGM;
        } else if (newState == GameState.GAME_START) {
            nextBGM = gameRunningBGM;
        } else if (newState == GameState.GAME_OVER) {
            nextBGM = gameOverBGM;
        } else {
            return;
        }

        StartCoroutine(FadeTo(nextBGM));
    }

    private IEnumerator FadeTo(AudioClip nextBGM) {
        yield return StartCoroutine(FadeOut());
        StartCoroutine(FadeIn(nextBGM));
    }

    private IEnumerator FadeIn(AudioClip nextBGM) {
        audioSource.volume = 0f;
        float timeElapsed = 0f;
        audioSource.resource = nextBGM;
        audioSource.Play();

        while (audioSource.volume < maxVolume) {
            audioSource.volume = Mathf.Lerp(0f, maxVolume, timeElapsed/fadeDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator FadeOut() {
        // audioSource.volume = volume;
        float timeElapsed = 0f;

        while (audioSource.volume > 0f && audioSource.isPlaying) {
            audioSource.volume = Mathf.Lerp(maxVolume, 0f, timeElapsed/fadeDuration);
            timeElapsed += Time.deltaTime;
            yield return false;
        }
        audioSource.Stop();
    }
}
