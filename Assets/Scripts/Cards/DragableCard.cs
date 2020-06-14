using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class DragableCard : BaseCard, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] protected GameObject _grayFilter;
    [SerializeField] private Image _cooldownFilter = default;
    [SerializeField] private SpriteRenderer _previewDoughnutPref = default;

    private bool _isRecharging;
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

        StartCoroutine(Recharge());
    }

    private void OnEnable() => GameManager.OnMoneyAmountChanged += AtualizeState;

    private void OnDisable() => GameManager.OnMoneyAmountChanged -= AtualizeState;

    public void AtualizeState(int moneyAmount)
    {
        if (moneyAmount >= _cardData.cost && !_isRecharging)
        {
            _grayFilter.SetActive(false);
        }
        else
        {
            _grayFilter.SetActive(true);
        }
    }

    private IEnumerator Recharge()
    {
        _isRecharging = true;

        float cooldownDuration = _cardData.cooldown > 0 ? _cardData.cooldown : 0.001f;

        float fill = 1f;
        _cooldownFilter.fillAmount = 1f;
        while (fill > 0)
        {
            float fillDelta = Time.deltaTime / cooldownDuration;

            fill -= fillDelta;
            _cooldownFilter.fillAmount -= fillDelta;
            yield return new WaitForSeconds(fillDelta);
        }

        _isRecharging = false;
        AtualizeState(GameManager.Instance.MoneyAmount);
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
        StartCoroutine(Recharge());
        GameManager.Instance.Withdraw(_cardData.cost);
    }

    private void ResetPosition()
    {
        _rectTransform.anchoredPosition = _initialAnchorPosition;
    }
    #endregion
}
