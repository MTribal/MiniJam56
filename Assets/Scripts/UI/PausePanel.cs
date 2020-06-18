using My_Utils.Audio;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{
    [SerializeField] private RectTransform _resumeButton = default;
    [SerializeField] private RectTransform _restartButton = default;
    [SerializeField] private RectTransform _menuButton = default;

    [Space]
    [SerializeField] private RectTransform _panelToFade = default;
    [SerializeField] private RectTransform _panelToScale = default;

    public void Resume()
    {
        Time.timeScale = 1f;
        AudioManager.Instance.SetVolume("SoundTrack", 1f);
        UI_Utils.Instance.ButtonPressedAnim(_resumeButton);
        UI_Utils.Instance.PanelCloseAnim(_panelToFade, _panelToScale, () => { _panelToFade.gameObject.SetActive(false); });
    }

    public void Restart()
    {
        AudioManager.Instance.SetVolume("SoundTrack", 1f);
        UI_Utils.Instance.ButtonPressedAnim(_restartButton);
        GameManager.Instance.LoadGame();
    }

    public void Menu()
    {
        AudioManager.Instance.SetVolume("SoundTrack", 1f);
        UI_Utils.Instance.ButtonPressedAnim(_menuButton);
        GameManager.Instance.LoadMainMenu();
    }
}
