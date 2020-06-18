using UnityEngine;

public class Credits : MonoBehaviour
{
    [SerializeField] private RectTransform _backButton = default;

    public void Back()
    {
        UI_Utils.Instance.ButtonPressedAnim(_backButton);
        GameManager.Instance.LoadMainMenu();
    }
}
