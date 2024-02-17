using UnityEngine;
using TMPro;

public class ScoreManager : Singleton<ScoreManager> {
    [SerializeField] private TMP_Text scoreDisplay;
    [SerializeField] private TMP_Text missesDisplay;
    private int score = 0;
    private int misses = 0;

    void Start() {
    }

    void OnEnable() {
        // just testing out static events.
        FishManager.fishCaughtEvent += IncrementScore;
        FishManager.fishDiedOfNaturalCausesEvent += IncrementMisses;
        score = 0;
        misses = 0;
        scoreDisplay.SetText(score.ToString());
        missesDisplay.SetText(score.ToString());
    }

    void OnDisable() {
        FishManager.fishCaughtEvent -= IncrementScore;
        FishManager.fishDiedOfNaturalCausesEvent -= IncrementMisses;
    }

    void Update() {
    }

    void IncrementScore(int amount) {
        score += amount;
        scoreDisplay.SetText(score.ToString());
    }

    void IncrementMisses() {
        misses++;
        missesDisplay.SetText(misses.ToString());
    }
}
