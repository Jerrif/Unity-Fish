using UnityEngine;
using System.Collections;


/*

*********
I MADE UP MY MIND, I'M GOING FULL EVENTS
*********

Okay, lets think here. The only SFX I'm going to have are:
* fish caught pops
* hook cast
* splash
* hook reel
* UI button hovered & pressed

UI:
The UI button sounds I think will just reside on the UI itself, no?
Either on the UI element (button) OR maaaaybe on the `UIManager`
In either case, they don't need to come through here at all I think
you just hook up an audio source to the button + an `OnPointerEnter` event
HOWEVER, it could also be done like:
`SFXManager` or `UIManager` holds the `AudioSource` and all the UI SFX
then you just hook up the `OnPointerEnter` event to call the `Play` func on here or `UIManager`


Hook cast / splash / reel:
most simple implementation would just be to have the sounds on the hook object
and it would call like `SFXManager.Instance.Play(castSFX.wmv)`
I guess in theory `SFXManager` could listen for each of the relevant hook events.
The sounds would all be on `SFXManager`, the hook wouldn't even know it's making sounds play.
That makes it """decoupled""" but what's the point?

Fish:
Here's the interesting one. I feel like normally you'd want the object to hold its own sfx
eg: monster hurt effect -> monster code would just play the sfx at the appropriate time
(it would probably still call `SFXManager.PlaySFX(myHurtSFX)` through the SFXManager I guess?)
BUT in my case the fish will all make the same sound (pop), and due to the nature 
of the game, they will all always play on the same frame.
But what I want is to slightly offset each fish's pop sounds so they play one after the other.
Because of this, I'm wondering if I should just keep the fish pop SFX on the SFXManager
and just have a separate (hardcoded) function for like `PlayFishPops(count)`.

To play the fish pops in sequence I'd need some kind of queue + a coroutine I guess.


FINAL NOTE:
Since `SFXManager` and `BGMManager` are both really small, maybe I should combine them
into `AudioManager`.



Interesting: https://www.reddit.com/r/Unity2D/comments/yhawxp/audio_sources_and_audio_clips/
```
My set up is I have a sfx manager and use Actions and Delegates. 
So I have my audio clip as a public variable in whatever object plays the sound,
and it fires an event that passes the clip into the manager.
I have one audio source for sfx that should play through, and one that interrupts,
because if you have a bunch of the same sound play at once, the volume is additive and goes nuts.
For the Full play action it just has the corresponding audio source play,
and if I need it to interrupt, it uses the other audio source and runs a Stop function first.
```
note: events are neat here because you can entirely remove/disable the sfx manager
and everything will still work (my BGMManager works this way right now).
*/


// NOTE: Usually the sfx would be on the object emitting the event, and passed in via the event:
// `public static event Action<SoundEffect> HookCastEvent;`
// but I'm already using the hook events elsewhere, so I'm just being lazy and
// putting the sfx on here. But that's how I *WOULD* do it.

[RequireComponent(typeof(AudioSource))]
public class SFXManager : MonoBehaviour {
    private AudioSource audioSource;
    [SerializeField] private float maxVolume;
    [SerializeField] private AudioClip hookCastSFX;
    [SerializeField] private AudioClip hookReelSFX;
    [SerializeField] private AudioClip hookLandedSFX;
    [SerializeField] private AudioClip fishCaughtSFX;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = maxVolume;
        // audioSource.loop = true;
    }

    private void OnEnable() {
        HookController.HookCastEvent              += OnHookCast;
        HookController.HookLandedEvent            += OnHookLanded;
        HookController.HookReelingFinishedEvent   += OnHookReelingFinished;
        FishManager.fishCaughtEvent               += OnFishCaught;
        GameManager.gameStateChanged              += OnGameStateChanged;
    }

    private void OnDisable() {
        HookController.HookCastEvent              -= OnHookCast;
        HookController.HookLandedEvent            -= OnHookLanded;
        HookController.HookReelingFinishedEvent   -= OnHookReelingFinished;
        FishManager.fishCaughtEvent               -= OnFishCaught;
        GameManager.gameStateChanged              -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newState) {
        audioSource.Stop();
    }

    private void OnHookCast(float hookCastingTime) {
        PlaySFX(hookCastSFX);
        // audioSource.clip = hookReelSFX;
        // audioSource.Play();
    }

    private void OnHookLanded() {
        PlaySFX(hookLandedSFX);
    }

    private void OnHookReelingFinished() {
        // audioSource.Stop();
    }

    private void OnFishCaught(int numCaught) {
        if (numCaught == 0) return;
        StartCoroutine(PlayPops(numCaught));
    }

    private IEnumerator PlayPops(int numCaught) {
        for (int i = 0; i < numCaught; i++) {
            float variation = Random.Range(-0.03f, 0f);
            yield return new WaitForSeconds(0.08f + variation);
            // yes, this changes the pitch for all other sfx too, but I kinda like that
            audioSource.pitch = 1f + Random.Range(-0.2f, 0.2f);
            PlaySFX(fishCaughtSFX);
        }
    }

    public void PlaySFX(AudioClip clip) {
        audioSource.PlayOneShot(clip);
    }
}
