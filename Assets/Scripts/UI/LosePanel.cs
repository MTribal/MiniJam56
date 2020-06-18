using My_Utils.Audio;
using UnityEngine;

public class LosePanel : MonoBehaviour
{
    [SerializeField] private RectTransform _restartButton = default;
    [SerializeField] private RectTransform _menuButton = default;

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
