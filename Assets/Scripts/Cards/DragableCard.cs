using My_Utils;
using My_Utils.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup), typeof(BaseCard))]
public class DragableCard : DragableObject, IDragable
{
    [SerializeField] protected GameObject _grayFilter;
    [SerializeField] private Image _cooldownFilter = default;
    [SerializeField] private SpriteRenderer _previewDoughnutPref = default;

    private BaseCard _baseCard;
    private bool _isRecharging;
    private SpriteRenderer _previewDoughnut;
    private Vector2 _initialAnchorPosition;

    private void OnEnable() => GameManager.OnMoneyAmountChanged += AtualizeState;

    private void OnDisable() => GameManager.OnMoneyAmountChanged -= AtualizeState;

    public void Initialize(CardData cardData)
    {
        if (_previewDoughnut != null) return; // Already initialized.

        _baseCard = GetComponent<BaseCard>();
        _baseCard.Initialize(cardData);

        _previewDoughnut = Instantiate(_previewDoughnutPref);
        _previewDoughnut.sprite = _baseCard.CardData.sprite;
        _previewDoughnut.gameObject.SetActive(false);
        _rectTransform = GetComponent<RectTransform>();
        _initialAnchorPosition = _rectTransform.anchoredPosition;

        StartCoroutine(Recharge());
    }

    public void AtualizeState(int moneyAmount)
    {
        if (moneyAmount >= _baseCard.CardData.cost && !_isRecharging)
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

        float cooldownDuration = _baseCard.CardData.cooldown > 0 ? _baseCard.CardData.cooldown : 0.001f;

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
    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);

        List<RaycastResult> objectsHit = MyUtils.MouseRaycastResults();
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

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);

        _previewDoughnut.gameObject.SetActive(false);
        ResetPosition();
    }

    private void ResetPosition()
    {
        _rectTransform.anchoredPosition = _initialAnchorPosition;
    }

    public void PutDoughnut(Vector2 position)
    {
        AudioManager.Instance.PlaySound("DropCard");
        Instantiate(_baseCard.CardData.doughnut, position, Quaternion.identity);
        ResetPosition();
        StartCoroutine(Recharge());
        GameManager.Instance.Withdraw(_baseCard.CardData.cost);
    }
    #endregion
}
