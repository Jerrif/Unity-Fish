using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))] 
public class ScrollBackground : MonoBehaviour {
    [SerializeField] private float _x, _y = 0.2f;
    RawImage img;

    void OnEnable() {
        img = GetComponent<RawImage>();
    }

    void Update() {
        img.uvRect = new Rect(img.uvRect.position + new Vector2(_x, _y) * Time.deltaTime, img.uvRect.size);
    }
}
