using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class DragableCard : BaseCard, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] protected GameObject _grayFilter;
    [SerializeField] private SpriteRenderer _previewDoughnutPref = default;

    private CardStates _cardState;
    private Canvas _canvas;
    private CanvasGroup _canvasGroup;
    private RectTransform _rectTransform;
    private SpriteRenderer _previewDoughnut;
    private Vector2 _initialAnchorPosition;

    public override void SetCardData(CardData cardData)
    {
        base.SetCardData(cardData);
        _previewDoughnut = Instantiate(_previewDoughnutPref);
        _previewDoughnut.sprite = _cardData.sprite;
        _previewDoughnut.gameObject.SetActive(false);

        _rectTransform = GetComponent<RectTransform>();
        _canvas = GameObject.FindGameObjectWithTag("PlayCanvas").GetComponent<Canvas>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _initialAnchorPosition = _rectTransform.anchoredPosition;
    }
    
    private void OnEnable() => GameManager.OnMoneyAmountChanged += AtualizeState;

    private void OnDisable() => GameManager.OnMoneyAmountChanged -= AtualizeState;

    public void AtualizeState(int moneyAmount)
    {
        if (moneyAmount >= _cardData.cost)
        {
            _cardState = CardStates.Buyable;
            _grayFilter.SetActive(false);
        }
        else
        {
            _cardState = CardStates.NotBuyable;
            _grayFilter.SetActive(true);
        }

    }

    #region Events
    public void OnBeginDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;


        PointerEventData cursor = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };
        List<RaycastResult> objectsHit = new List<RaycastResult>();
        EventSystem.current.RaycastAll(cursor, objectsHit);

        bool hitSlot = false;
        for (int i = 0; i < objectsHit.Count; i++)
        {
            if (objectsHit[i].gameObject.GetComponent<TileSlot>() != null)
            {
                hitSlot = true;
                _previewDoughnut.transform.position = new Vector2(objectsHit[i].gameObject.transform.position.x, objectsHit[i].gameObject.transform.position.y);
            }
        }
        _previewDoughnut.gameObject.SetActive(hitSlot);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _canvasGroup.blocksRaycasts = true;
        _previewDoughnut.gameObject.SetActive(false);
        ResetPosition();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _canvasGroup.alpha = 0.6f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _canvasGroup.alpha = 1f;
    }

    public void PutDoughnut(Vector2 position)
    {
        Instantiate(_cardData.doughnut, position, Quaternion.identity);
        ResetPosition();
        GameManager.Instance.Withdraw(_cardData.cost);
    }

    private void ResetPosition()
    {
        _rectTransform.anchoredPosition = _initialAnchorPosition;
    }
    #endregion
}
