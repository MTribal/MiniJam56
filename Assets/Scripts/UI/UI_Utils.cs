using My_Utils.Audio;
using My_Utils.Lean_Tween;
using System;
using UnityEngine;

public class UI_Utils : MonoBehaviour
{
    public static UI_Utils Instance;

    [Header("Button Pressed Tween")]
    [SerializeField] private float _scaleTo = 0.9f;
    [SerializeField] private float _buttonAnimDuration = 0.1f;

    [Header("Panel Tween")]
    [SerializeField] private float _panelNormalAlpha = 0.6f;
    [SerializeField] private float _panelTweenDuration = 0.5f;
    [SerializeField] private LeanTweenType _panelOpenTween = LeanTweenType.EaseInBack;
    [SerializeField] private LeanTweenType _panelCloseTween = LeanTweenType.EaseInBack;

    public float DefaultButtonScaleTo { get => _scaleTo; }
    public float DefaultButtonTweenDuration { get => _buttonAnimDuration; }
    public float DefaultPanelNormalAlpha { get => _panelNormalAlpha; }
    public float DefaultPanelTweenDuration { get => _panelTweenDuration; }
    public LeanTweenType DefaultPanelOpenTweenType { get => _panelOpenTween; }
    public LeanTweenType DefaultPanelCloseTweenType { get => _panelCloseTween; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public LTDescr ButtonPressedAnim(RectTransform button)
    {
        AudioManager.Instance.PlaySound("Button");
        return LeanTween.Scale(button, new Vector3(_scaleTo, _scaleTo, _scaleTo), _buttonAnimDuration).SetEase(LeanTweenType.EaseInBack).SetRepeat(-1).SetLoopPingPong(1);
    }

    public void PanelCloseAnim(RectTransform panelToFade, RectTransform panelToScale)
    {
        LeanTween.Alpha(panelToFade, 0f, _panelTweenDuration, false).SetEase(_panelCloseTween);
        LeanTween.Scale(panelToScale, new Vector3(0f, 0f, 0f), _panelTweenDuration).SetEase(_panelCloseTween);
    }

    public void PanelCloseAnim(RectTransform panelToFade, RectTransform panelToScale, Action onComplete)
    {
        LeanTween.Alpha(panelToFade, 0f, _panelTweenDuration, false).SetEase(_panelCloseTween);
        LeanTween.Scale(panelToScale, new Vector3(0f, 0f, 0f), _panelTweenDuration).SetEase(_panelCloseTween).SetOnComplete(onComplete);
    }

    public void PanelCloseAnim(RectTransform panelToFade, RectTransform panelToScale, float tweenDuration, LeanTweenType tweenType)
    {
        LeanTween.Alpha(panelToFade, 0f, tweenDuration, false).SetEase(tweenType);
        LeanTween.Scale(panelToScale, new Vector3(0f, 0f, 0f), tweenDuration).SetEase(tweenType);
    }

    public void PanelCloseAnim(RectTransform panelToFade, RectTransform panelToScale, float tweenDuration, LeanTweenType tweenType, Action onComplete)
    {
        LeanTween.Alpha(panelToFade, 0f, tweenDuration, false).SetEase(tweenType);
        LeanTween.Scale(panelToScale, new Vector3(0f, 0f, 0f), tweenDuration).SetEase(tweenType).SetOnComplete(onComplete);
    }

    public void PanelOpenAnim(RectTransform panelToFade, RectTransform panelToScale)
    {
        LeanTween.Alpha(panelToFade, _panelNormalAlpha, _panelTweenDuration, false).SetEase(_panelOpenTween);
        LeanTween.Scale(panelToScale, new Vector3(1f, 1f, 1f), _panelTweenDuration).SetEase(_panelOpenTween);
    }


    public void PanelOpenAnim(RectTransform panelToFade, RectTransform panelToScale, Action onComplete)
    {
        LeanTween.Alpha(panelToFade, _panelNormalAlpha, _panelTweenDuration, false).SetEase(_panelOpenTween);
        LeanTween.Scale(panelToScale, new Vector3(1f, 1f, 1f), _panelTweenDuration).SetEase(_panelOpenTween).SetOnComplete(onComplete);
    }

    public void PanelOpenAnim(RectTransform panelToFade, RectTransform panelToScale, float panelAlpha, float tweenDuration, LeanTweenType tweenType)
    {
        LeanTween.Alpha(panelToFade, panelAlpha, tweenDuration, false).SetEase(tweenType);
        LeanTween.Scale(panelToScale, new Vector3(1f, 1f, 1f), tweenDuration).SetEase(tweenType);
    }


    public void PanelOpenAnim(RectTransform panelToFade, RectTransform panelToScale, float panelAlpha, float tweenDuration, LeanTweenType tweenType, Action onComplete)
    {
        LeanTween.Alpha(panelToFade, panelAlpha, tweenDuration, false).SetEase(tweenType);
        LeanTween.Scale(panelToScale, new Vector3(1f, 1f, 1f), tweenDuration).SetEase(tweenType).SetOnComplete(onComplete);
    }
}