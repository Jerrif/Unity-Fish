using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour {
    [SerializeField] private TMP_Text scoreDisplay;
    [SerializeField] private TMP_Text missesDisplay;
    private int score = 0;
    private int misses = 0;

    void Start() {
        // just testing out static events.
        GameManager.fishCaughtEvent += IncreaseScore;
        GameManager.fishDiedOfNaturalCausesEvent += IncreaseMisses;
        scoreDisplay.SetText("0");
        missesDisplay.SetText("0");
    }

    void OnDisable() {
        GameManager.fishCaughtEvent -= IncreaseScore;
        GameManager.fishDiedOfNaturalCausesEvent += IncreaseMisses;
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
