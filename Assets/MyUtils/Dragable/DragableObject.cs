using UnityEngine;
using UnityEngine.EventSystems;

namespace My_Utils
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class DragableObject : MonoBehaviour, IDragable
    {
        protected RectTransform _rectTransform;
        protected CanvasGroup _canvasGroup;
        private Canvas _canvas;

        protected virtual void Awake()
        {
            _canvas = GetComponentInParent<Canvas>();
            _canvasGroup = GetComponent<CanvasGroup>();
            _rectTransform = GetComponent<RectTransform>();
        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = false;
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = true;
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            OnPointerDownEffect();
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            OnPointerUpEffect();
        }

        /// <summary>
        /// Effect called when OnPointerDown is called. Override this to change the effect. (By default changing CanvasGroup alpha)
        /// </summary>
        protected virtual void OnPointerDownEffect()
        {
            _canvasGroup.alpha = 0.6f;
        }

        /// <summary>
        /// Effect called when OnPointerUp is called. Override this to change the effect. (By default changing CanvasGroup alpha)
        /// </summary>
        protected virtual void OnPointerUpEffect()
        {
            _canvasGroup.alpha = 1f;
        }
    }
}
