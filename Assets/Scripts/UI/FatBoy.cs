using My_Utils.Audio;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class FatBoy : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Tooltip("The alpha percentage when dragging the object.")]
    [SerializeField] private float _alphaWhenDragging = 0.6f;

    private float _normalAlpha;
    private CanvasGroup _canvasGroup;
    private RectTransform _rectTransform;
    private Canvas _canvas;
    private Vector2 _initialAnchorPosition;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _rectTransform = GetComponent<RectTransform>();
        _canvas = GameObject.FindGameObjectWithTag("PlayCanvas").GetComponent<Canvas>();
        _initialAnchorPosition = _rectTransform.anchoredPosition;
    }

    #region Events
    public void OnBeginDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = true;
        //_previewDoughnut.gameObject.SetActive(false);
        ResetPosition();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _normalAlpha = _canvasGroup.alpha;
        _canvasGroup.alpha = _alphaWhenDragging;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _canvasGroup.alpha = _normalAlpha;
    }

    public void RemoveDoughnut(Doughnut doughnut)
    {
        AudioManager.Instance.PlaySound("FatBoyEating");
        doughnut.DestroyItSelf();
        ResetPosition();
    }

    private void ResetPosition()
    {
        _rectTransform.anchoredPosition = _initialAnchorPosition;
    }
    #endregion
}
