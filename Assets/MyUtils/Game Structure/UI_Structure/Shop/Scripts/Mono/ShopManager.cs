using UnityEngine;
using System.Linq;

namespace My_Utils
{
    /// <summary>
    /// Manage all ShopItem assets and load saved data of the shop.
    /// </summary>
    public class ShopManager : MonoBehaviour
    {
        [Tooltip("A instance of this class.")]
        public static ShopManager instance;

        [SerializeField]
        [Tooltip("The list of all itens in the store.")]
        internal ShopItem[] shopItems = new ShopItem[0];

        private void Awake()
        {
            Undestructable();
            RegisterItensIds();
        }

        private void Undestructable()
        {
            if (instance == null)
            {
                instance = this;
                if (transform.parent == null)
                {
                    DontDestroyOnLoad(gameObject);
                }
                else
                {
                    DontDestroyOnLoad(transform.root);
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Register all itens in the shop (given in shopItems variable) and organize them by ID.
        /// </summary>
        private void RegisterItensIds()
        {
            for (int i = 0; i < shopItems.Length; i++)
            {
                shopItems[i].id = i;
            }
        }


        /// <summary>
        /// Load all shop data to shop
        /// </summary>
        /// <param name="shopData"> The saved ShopData </param>
        public void LoadShopData(ShopData shopData)
        {
            for (int i = 0; i < shopItems.Length; i++)
            {
                shopItems[i].itemStatus = shopData.itensStatus[i];
            }
        }


        /// <summary>
        /// Set status as Selected
        /// </summary>
        /// <param name="itemId"></param>
        public void SelectItem(int itemId)
        {
            // Deselect all
            for (int i = 0; i < shopItems.Length; i++)
            {
                if (shopItems[i].itemStatus == ItemStatus.Selected)
                {
                    shopItems[i].itemStatus = ItemStatus.Bought;
                }
            }

            // Select one
            shopItems.First(item => item.id == itemId).itemStatus = ItemStatus.Selected;
        }
    }
}
