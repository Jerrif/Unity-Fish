using UnityEngine;

public class FPSLimit : MonoBehaviour {
    public int targetFramerate = 155;

    private void Start() {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = targetFramerate;
    }
}