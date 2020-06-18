using UnityEngine.EventSystems;

namespace My_Utils
{
    public interface IDragable : IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {

    }
}
