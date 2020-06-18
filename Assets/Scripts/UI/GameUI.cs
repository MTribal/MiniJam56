using My_Utils.Audio;
using My_Utils.Lean_Tween;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private RectTransform _pauseButton = default;
    [SerializeField] private RectTransform _panelToFade = default;
    [SerializeField] private RectTransform _panelToScale = default;

    public void Pause()
    {
        UI_Utils.Instance.ButtonPressedAnim(_pauseButton);
        _panelToFade.gameObject.SetActive(true);
        UI_Utils.Instance.PanelOpenAnim(_panelToFade, _panelToScale, () =>
        {
            AudioManager.Instance.SetVolume("SoundTrack", 0.4f);
            Time.timeScale = 0f;
        }); ;
    }
}
