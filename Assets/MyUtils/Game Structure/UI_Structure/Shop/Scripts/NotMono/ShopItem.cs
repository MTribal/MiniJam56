//
// DON'T USE THE SAME ASSET IN TWO DIFFERENT PLACES IN YOUR PROJECT
//
using UnityEngine;

namespace My_Utils
{
    /// <summary>
    /// ShopItem is the script of all ShopItem assets in the game.
    /// </summary>
    [CreateAssetMenu(fileName = "newShopItem", menuName = "ShopItem")]
    public class ShopItem : ScriptableObject
    {
        [Tooltip("The icon sprite of this item.")]
        public Sprite itemIcon;
        [Tooltip("Mark to preserve itemIcon image aspect.")]
        public bool preserveIconAspect;

        [Space]

        [Tooltip("The name of this item.")]
        public string itemName;

        [Tooltip("The price of this item.")]
        public float price;

        [Space]

        [Tooltip("The currency icon sprite of this item.")]
        public Sprite currencyIcon;
        [Tooltip("Mark to preserve currencyIcon image aspect.")]
        public bool preserveCurrencyAspect;

        [Header("Opcional parameters")]

        [TextArea(5, 10)]
        [Tooltip("The info description of this item. (Opcional, depends on ItemModel you'll choose)")]
        public string description;

        /// <summary>
        /// This value is controlled by ShopManager and is for manage all itens properly.
        /// </summary>
        internal int id;

        /// <summary>
        /// This value is controlled by ShopManager and is the atual status of 
        /// </summary>
        public ItemStatus itemStatus = ItemStatus.OnSale;
    }
}
