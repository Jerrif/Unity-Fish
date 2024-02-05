using System;
using UnityEngine;

public class Timer : MonoBehaviour {
    private float _seconds;
    private bool isStarted = false;

    public event Action timerExpired;

    private void Start() {
        
    }

    public void StartTimer(float seconds) {
        _seconds = seconds;
        isStarted = true;
    }

    private void Update() {
        if (!isStarted) {
            return;
        }
        if (_seconds >= 0f) {
            _seconds -= Time.deltaTime;
            return;
        }
        _seconds = 0f;
        isStarted = false;
        timerExpired?.Invoke();
    }
}
