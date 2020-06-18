using TMPro;
using UnityEngine;

public class ScoreField : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreAmountText = default;

    private void Start() => Atualize(GameManager.Instance.AtualScore);

    private void OnEnable() => GameManager.OnScoreChanged += Atualize;

    private void OnDisable() => GameManager.OnScoreChanged += Atualize;

    private void Atualize(int scoreAmount)
    {
        _scoreAmountText.text = scoreAmount.ToString();
    }
}
