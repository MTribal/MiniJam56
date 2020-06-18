using My_Utils;
using My_Utils.Audio;
using UnityEngine;

public class GameOverLine : MonoBehaviour
{
    [SerializeField] private RectTransform _panelToFade = default;
    [SerializeField] private RectTransform _panelToScale = default;
    [SerializeField] private LayerMask _enemyLayer = default;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_enemyLayer.Contains(collision.gameObject.layer))
        {
            AudioManager.Instance.SetVolume("SoundTrack", 0.4f);
            GameManager.Instance.GameOver(_panelToFade, _panelToScale);
        }
    }
}
