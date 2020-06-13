using UnityEngine;
using UnityEngine.UI;

namespace My_Utils
{
    /// <summary>
    /// Instantiate all itens of a section, based in a ShopItem array.
    /// </summary>
    [ExecuteInEditMode]
    [RequireComponent(typeof(GridLayoutGroup))]
    public class ItensGridManager : MonoBehaviour
    {
        /// <summary>
        /// The RectTransform component attached to this object
        /// </summary>
        private RectTransform rectTransform;

        /// <summary>
        /// The GridLayoutGroup component attached to this object
        /// </summary>
        private GridLayoutGroup grid;

        [Tooltip("If marked will calculate grid's width automatically based on cell size and padding.")]
        public bool automaticWidth;

        [SerializeField]
        [Tooltip("The prefab model to instantiate all itens.")]
        private ShopItemPrefab itemModel = null;

        [Space]

        [Tooltip("Click to atualize all itens.")]
        public bool atualize;

        [SerializeField]
        [Tooltip("The itens (ShopItem asset) that will sell in the store.")]
        private ShopItem[] shopItens = new ShopItem[] { };

        private void Update()
        {
            if (atualize)
            {
                atualize = false;
                AtualizeItens();
            }
            if (automaticWidth)
            {
                CalculateGridWidth();
            }
        }

        private void OnValidate()
        {
            rectTransform = GetComponent<RectTransform>();
            grid = GetComponent<GridLayoutGroup>();
        }

        public void AtualizeItens()
        {
            DeleteAllItens();
            InstantiateItens();
        }

        private void DeleteAllItens()
        {
            GameObject[] childs = new GameObject[transform.childCount];

            for (int i = 0; i < childs.Length; i++)
            {
                childs[i] = transform.GetChild(i).gameObject;
            }

            for (int i = 0; i < childs.Length; i++)
            {
                DestroyImmediate(childs[i]);
            }
        }

        private void InstantiateItens()
        {
            for (int i = 0; i < shopItens.Length; i++)
            {
                if (shopItens[i] != null)
                {
                    ShopItemPrefab instantiatedItem = Instantiate(itemModel, transform);

#if UNITY_EDITOR
                    #region LogExceptions

                    // itemModel null exceptions
                    MyUtils.TreatNullExceptions(instantiatedItem.itemIconImage, "itemIconImage", "ItemModel");
                    MyUtils.TreatNullExceptions(instantiatedItem.itemNameLabel, "itemNameLabel", "ItemModel");
                    MyUtils.TreatNullExceptions(instantiatedItem.itemPriceLabel, "itemPriceLabel", "ItemModel");
                    MyUtils.TreatNullExceptions(instantiatedItem.currencyIcon, "currencyIcon", "ItemModel");

                    // shopIten[i] null exceptions
                    MyUtils.TreatNullExceptions(shopItens[i].itemIcon, "itemIcon", shopItens[i].name);
                    MyUtils.TreatNullExceptions(shopItens[i].itemName, "itemName", shopItens[i].name);
                    MyUtils.TreatNullExceptions(shopItens[i].currencyIcon, "currencyIcon", shopItens[i].name);

                    #endregion
#endif              

                    instantiatedItem.id = shopItens[i].id;
                    instantiatedItem.itemStatus = shopItens[i].itemStatus;

                    // Opcional
                    if (instantiatedItem.itemInfoLabel != null) // InfoLabel is opcional
                    {
                        instantiatedItem.itemInfoLabel.text = shopItens[i].description;
                    }

                    // Anyway
                    instantiatedItem.itemIconImage.sprite = shopItens[i].itemIcon;
                    instantiatedItem.itemIconImage.preserveAspect = shopItens[i].preserveIconAspect;
                    instantiatedItem.itemNameLabel.text = shopItens[i].itemName;

                    if (shopItens[i].itemStatus == ItemStatus.OnSale)
                    {
                        instantiatedItem.border.color = instantiatedItem.onSaleBorder;

                        instantiatedItem.itemPriceLabel.text = shopItens[i].price.ToString();
                        instantiatedItem.currencyIcon.sprite = shopItens[i].currencyIcon;
                        instantiatedItem.currencyIcon.preserveAspect = shopItens[i].preserveCurrencyAspect;
                        instantiatedItem.buyButton.gameObject.SetActive(true);
                        instantiatedItem.selectButton.gameObject.SetActive(false);

                        instantiatedItem.buyPanelItemIcon.sprite = shopItens[i].itemIcon;
                        instantiatedItem.buyPanelItemIcon.preserveAspect = shopItens[i].preserveIconAspect;
                        instantiatedItem.buyPanelPrice.text = shopItens[i].price.ToString();
                        instantiatedItem.buyPanelCurrencyIcon.sprite = shopItens[i].currencyIcon;
                        instantiatedItem.buyPanelCurrencyIcon.preserveAspect = shopItens[i].preserveCurrencyAspect;
                    }
                    else
                    {
                        RectTransform iconRect = instantiatedItem.itemIconImage.rectTransform;
                        RectTransform nameRect = instantiatedItem.itemNameLabel.rectTransform;
                        RectTransform priceRect = instantiatedItem.itemPriceLabel.rectTransform.parent.GetComponent<RectTransform>();
                        CentralizeIcon(iconRect, nameRect, priceRect);

                        // Grow Icon
                        iconRect.sizeDelta *= instantiatedItem.iconGrowPercentage + 1;

                        instantiatedItem.itemPriceLabel.gameObject.SetActive(false);
                        instantiatedItem.currencyIcon.gameObject.SetActive(false);
                        instantiatedItem.buyButton.gameObject.SetActive(false);

                        instantiatedItem.buyPanelItemIcon.gameObject.SetActive(false);
                        instantiatedItem.buyPanelPrice.gameObject.SetActive(false);
                        instantiatedItem.buyPanelCurrencyIcon.gameObject.SetActive(false);

                        if (shopItens[i].itemStatus == ItemStatus.Selected)
                        {
                            instantiatedItem.border.color = instantiatedItem.selectedBorder;
                        }
                        else // itemStatus == ItemStatus.Bought
                        {
                            instantiatedItem.border.color = instantiatedItem.boughtBorder;

                            instantiatedItem.selectButton.gameObject.SetActive(true);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Centralize item icon image. Use when status is bought
        /// </summary>
        /// <param name="iconRect">Icon RectTransform</param>
        /// <param name="nameRect">Name label RectTransform</param>
        /// <param name="priceRect">Price label RectTransform</param>
        private void CentralizeIcon(RectTransform iconRect, RectTransform nameRect, RectTransform priceRect)
        {
            float positiveGap = nameRect.anchoredPosition.y - (nameRect.sizeDelta.y / 2);
            float negativeGap = grid.cellSize.y / -2;
            float max = Mathf.Max(Mathf.Abs(positiveGap), Mathf.Abs(negativeGap));
            float sum = Mathf.Abs(positiveGap) + Mathf.Abs(negativeGap);

            float posY;
            if (max == positiveGap)
            {
                posY = negativeGap - (sum / 2);
            }
            else
            {
                posY = positiveGap - (sum / 2);
            }

            iconRect.anchoredPosition = new Vector2(iconRect.anchoredPosition.x, posY);
        }


        /// <summary>
        /// Calculate RectTransform width 
        /// </summary>
        private void CalculateGridWidth()
        {
            float width = (grid.padding.left + grid.padding.right) + (shopItens.Length * grid.cellSize.x) + ((shopItens.Length - 1) * grid.spacing.x);
            rectTransform.sizeDelta = new Vector2(width, rectTransform.sizeDelta.y);
        }


        /// <summary>
        /// Select the given item
        /// </summary>
        /// <param name="itemToSelect"></param>
        public void Select(ShopItemPrefab itemToSelect)
        {
            // Deselect all
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<ShopItemPrefab>().Deselect();
            }

            // Select one
            itemToSelect.border.color = itemToSelect.selectedBorder;
            itemToSelect.itemStatus = ItemStatus.Selected;

            // Atualize ShopManager
            ShopManager.instance.SelectItem(itemToSelect.id);
        }
    }
}
