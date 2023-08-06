using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Shop/Shop System")]
public class ShopSystem : ScriptableObject
{
    [SerializeField] ShopItem[] shopItems;

    public ShopItem[] GetShopItems() { return shopItems; }

    public bool TryPurchase(ShopItem shopItem, CreditComponent purchaser)
    {
        return purchaser.Purchase(shopItem.price, shopItem.item);
    }
}
