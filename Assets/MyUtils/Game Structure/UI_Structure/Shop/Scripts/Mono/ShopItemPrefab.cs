using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace My_Utils
{
    /// <summary>
    /// Contain information and functions used in a model of item
    /// </summary>
    public class ShopItemPrefab : MonoBehaviour
    {
        [Tooltip("Icon grow percentage when is bought.")]
        [Range(0, 1)]
        public float iconGrowPercentage = 0.125f;

        [Tooltip("The icon image component(UI Image) in this prefab.")]
        public Image itemIconImage;

        [Tooltip("The item name text component(TextMeshPro) in this prefab.")]
        public TextMeshProUGUI itemNameLabel;

        [Tooltip("The item price text component(TextMeshPro) in this prefab.")]
        public TextMeshProUGUI itemPriceLabel;

        [Tooltip("The currency icon image component(UI Image) in this prefab.")]
        public Image currencyIcon;

        [Tooltip("BuyButton (Button)component in this prefab.")]
        public Button buyButton;

        [Tooltip("SelectButton (Button)component in this prefab.")]
        public Button selectButton;


        [Header("Border")]

        [Tooltip("The border Image component in this prefab.")]
        public Image border;

        [Tooltip("Border color when item status OnSale.")]
        public Color onSaleBorder;

        [Tooltip("Border color when item status Bought.")]
        public Color boughtBorder;

        [Tooltip("Border color when item status Selected.")]
        public Color selectedBorder;


        [Header("BuyPanel")]

        [Tooltip("The icon image component (UI Image) in BuyPanel in this prefab.")]
        public Image buyPanelItemIcon;

        [Tooltip("The item price text component (TextMeshPro) in BuyPanel in this prefab.")]
        public TextMeshProUGUI buyPanelPrice;

        [Tooltip("The currency icon image component (UI Image) in BuyPanel in this prefab.")]
        public Image buyPanelCurrencyIcon;


        [Header("Opcional parameters")]

        [Tooltip("The info text component (TextMeshPro) in this prefab. (Opcional, this can be null)")]
        public TextMeshProUGUI itemInfoLabel;

        /// <summary>
        /// Id (corresponding to ShopItem id) of this item.
        /// </summary>
        internal int id = -1;

        /// <summary>
        /// The atual status of this item
        /// </summary>
        internal ItemStatus itemStatus;

        public void SelectButton()
        {
            GetComponentInParent<ItensGridManager>().Select(this);
        }

        /// <summary>
        /// Deselect this item case it's selected
        /// </summary>
        public void Deselect()
        {
            if (itemStatus == ItemStatus.Selected)
            {
                itemStatus = ItemStatus.Bought;
                border.color = boughtBorder;
            }
        }
    }
}
