using UnityEngine;
using UnityEngine.UI;

namespace My_Utils
{
    /// <summary>
    /// Contain a function to change active section in the shop.
    /// </summary>
    [RequireComponent(typeof(Toggle))]
    public class StoreSectionToggle : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The ScrollRect component (ScrollView object) that this toggle controls.")]
        private ScrollRect scrollRect = null;

        public void ClickedOnSection(bool selected)
        {
            FindObjectOfType<ShowcaseAtualizer>().AtualizeAll();
            scrollRect.gameObject.SetActive(selected);
            if (selected)
            {
                scrollRect.horizontalNormalizedPosition = 0f; // Reset scrolled position to zero
            }
        }
    }
}
