using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private RectTransform _playButton = default;
    [SerializeField] private RectTransform _creditsButton = default;

    public void Play()
    {
        UI_Utils.Instance.ButtonPressedAnim(_playButton);
        GameManager.Instance.LoadGame();
    }

    public void Credits()
    {
        UI_Utils.Instance.ButtonPressedAnim(_creditsButton);
        GameManager.Instance.LoadCredits();
    }
}
