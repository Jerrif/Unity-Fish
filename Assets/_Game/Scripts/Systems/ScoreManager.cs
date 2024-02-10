using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour {
    [SerializeField] private TMP_Text scoreDisplay;
    [SerializeField] private TMP_Text missesDisplay;
    private int score = 0;
    private int misses = 0;

    void Start() {
        // just testing out static events.
        FishManager.fishCaughtEvent += IncrementScore;
        FishManager.fishDiedOfNaturalCausesEvent += IncrementMisses;
        scoreDisplay.SetText("0");
        missesDisplay.SetText("0");
    }

    void OnDisable() {
        FishManager.fishCaughtEvent -= IncrementScore;
        FishManager.fishDiedOfNaturalCausesEvent += IncrementMisses;
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
