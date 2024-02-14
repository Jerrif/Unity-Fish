using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickyButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    [SerializeField] private Image _img;
    [SerializeField] private Sprite _default, _pressed;
    [SerializeField] private AudioClip _compressClip, _uncompressClip;
    [SerializeField] private AudioSource _source;

    public void OnPointerDown(PointerEventData eventData) {
        _img.sprite = _pressed;
        // _source.PlayOneShot(_compressClip);
    }

    public void OnPointerUp(PointerEventData eventData) {
        _img.sprite = _default;
        // _source.PlayOneShot(_uncompressClip);
    }

    // TODO: yeah, this needs to be changed. These scene should be changed from a GameManager / UIManager?
    public void IWasClicked() {
        // probably not how it's done? idk, just testing for now
        GameManager.Instance.UpdateGameState(GameState.GAME_START);
        SceneSystem.Instance.LoadNextScene();
    }
}
