namespace My_Utils
{
    /// <summary>
    /// Store all shop data of the game.
    /// </summary>

    [System.Serializable]
    public class ShopData : Data
    {
        internal ItemStatus[] itensStatus;

        public ShopData()
        {
            itensStatus = new ItemStatus[ShopManager.instance.shopItems.Length];

            for (int i = 0; i < itensStatus.Length; i++)
            {
                itensStatus[i] = ShopManager.instance.shopItems[i].itemStatus;
            }
        }
    }
}
