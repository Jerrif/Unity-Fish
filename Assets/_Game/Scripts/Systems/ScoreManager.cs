using UnityEngine;
using TMPro;

public class ScoreManager : Singleton<ScoreManager> {
    [SerializeField] private TMP_Text scoreDisplay;
    [SerializeField] private TMP_Text missesDisplay;
    private int score = 0;
    private int misses = 0;

    private void OnEnable() {
        // just testing out static events.
        FishManager.fishCaughtEvent += IncrementScore;
        FishManager.fishDiedOfNaturalCausesEvent += IncrementMisses;
        Init();
    }

    private void OnDisable() {
        FishManager.fishCaughtEvent -= IncrementScore;
        FishManager.fishDiedOfNaturalCausesEvent -= IncrementMisses;
    }

    public void IncrementScore(int amount) {
        score += amount;
        scoreDisplay.SetText(score.ToString());
    }

    public void IncrementMisses() {
        misses++;
        missesDisplay.SetText(misses.ToString());
    }

    public void Init() {
        score = 0;
        misses = 0;
        scoreDisplay.SetText("0");
        missesDisplay.SetText("0");
    }
}
