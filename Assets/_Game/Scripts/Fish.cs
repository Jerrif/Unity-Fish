using System;
using UnityEngine;

public class Fish : MonoBehaviour {
    [SerializeField] float speed = 1f;
    private float distanceToTravel = 15f;

    private Vector2 startPos;
    SpriteRenderer sprite;
    // NOTE: pass a ref to the fish that died so the `FishSpawner` can remove it from the fish array
    // is this silly/overcomplicated?
    public event Action<Fish> died;

    void Start() {
        startPos = transform.position;
        sprite = GetComponent<SpriteRenderer>();
        // Bounds pos = sprite.bounds;
    }

    void Update() {
        transform.position += Vector3.left * Time.deltaTime * speed;
        // Bounds pos = sprite.bounds;
        // print(pos);
        if (distanceTravelled() >= distanceToTravel) {
            // BUG: can't do this; the FishSpawner holds an array with a ref to each fish
            Died();
            Destroy(gameObject);
        }
    }

    float distanceTravelled() {
        // if (transform.position.x - startPos.x >= distanceToTravel) {

        // }
        return Mathf.Abs(transform.position.x - startPos.x);
    }

    void Died() {
        print("wow a fish died (from fish script)");
        died?.Invoke(this);
    }
}
