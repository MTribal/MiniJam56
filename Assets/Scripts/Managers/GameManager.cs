using My_Utils;
using My_Utils.Audio;
using UnityEngine;
using UnityEngine.UI;

public delegate void ReceiveAmount(int amount); 

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public bool IsPlaying { get; private set; } = true;

    [Header("Game")]
    [Tooltip("The amount of money that player starts with.")]
    [SerializeField] private int _initialMoney = default;
    [Tooltip("Multuply score by that before adding.")]
    [SerializeField] private int _scoreMultiplier = default;

    [Header("UI")]
    [SerializeField] private string _mainMenuSceneName = default;
    [SerializeField] private string _creditsSceneName = default;
    [SerializeField] private string _gameSceneName = default;

    /// <summary>
    /// Called when atual money amount changed.
    /// </summary>
    public static event ReceiveAmount OnMoneyAmountChanged;

    /// <summary>
    /// The atual money amount.
    /// </summary>
    public int MoneyAmount 
    {
        get
        {
            return _moneyAmount;
        }

        private set
        {
            _moneyAmount = value;
            OnMoneyAmountChanged?.Invoke(_moneyAmount);
        }
    }
    private int _moneyAmount;


    /// <summary>
    /// Called when atual score changed.
    /// </summary>
    public static event ReceiveAmount OnScoreChanged;

    /// <summary>
    /// The score  amount.
    /// </summary>
    public int AtualScore
    {
        get
        {
            return _atualScore;
        }

        private set
        {
            _atualScore = value;
            OnScoreChanged?.Invoke(_atualScore);
        }
    }
    private int _atualScore;
    

    public void AddScore(int amount)
    {
        AtualScore += amount * _scoreMultiplier;
    }

    public void Withdraw(int amount)
    {
        MoneyAmount -= amount;
    }

    public void Deposit(int amount)
    {
        MoneyAmount += amount;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            AudioManager.Instance.PlaySound("SoundTrack");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GameOver(RectTransform panelToFade, RectTransform panelToScale)
    {
        if (!IsPlaying) return;

        IsPlaying = false;
        panelToFade.gameObject.SetActive(true);
        UI_Utils.Instance.PanelOpenAnim(panelToFade, panelToScale, () => { Time.timeScale = 0f; });
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneLoader.Instance.LoadScene(_mainMenuSceneName, true);
    }

    public void LoadCredits()
    {
        SceneLoader.Instance.LoadScene(_creditsSceneName, true);
    }

    public void LoadGame()
    {
        IsPlaying = true;
        AtualScore = 0;
        MoneyAmount = _initialMoney;    
        Time.timeScale = 1f;
        SceneLoader.Instance.LoadScene(_gameSceneName, true);
    }
}
