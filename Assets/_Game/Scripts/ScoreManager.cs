using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour {
    [SerializeField] private TMP_Text scoreDisplay;
    [SerializeField] private TMP_Text missesDisplay;
    private int score = 0;
    private int misses = 0;

    void Start() {
        // just testing out static events.
        FishManager.fishCaughtEvent += IncreaseScore;
        FishManager.fishDiedOfNaturalCausesEvent += IncreaseMisses;
        scoreDisplay.SetText("0");
        missesDisplay.SetText("0");
    }

    void OnDisable() {
        FishManager.fishCaughtEvent -= IncreaseScore;
        FishManager.fishDiedOfNaturalCausesEvent += IncreaseMisses;
    }

    void Update() {
    }

    void IncreaseScore(int amount) {
        score += amount;
        scoreDisplay.SetText(score.ToString());
    }

    void IncreaseMisses() {
        misses++;
        missesDisplay.SetText(misses.ToString());
    }
}
