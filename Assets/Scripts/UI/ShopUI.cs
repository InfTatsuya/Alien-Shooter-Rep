using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [SerializeField] ShopSystem shopSystem;
    [SerializeField] ShopItemUI shopItemUIPrefab;
    [SerializeField] RectTransform shopList;

    [SerializeField] CreditComponent creditComponent;

    [SerializeField] UIManager uiManager;

    [SerializeField] TextMeshProUGUI creditText;
    [SerializeField] Button backButton;
    [SerializeField] Button buyButton;

    private List<ShopItemUI> shopItems = new List<ShopItemUI>();
    private ShopItemUI selectedShopItemUI;

    private void Start()
    {
        InitShopItems();

        backButton.onClick.AddListener(uiManager.SwitchToGameplayUI);
        buyButton.onClick.AddListener(TryPurchaseItem);
        creditComponent.onCreditChanged += CreditComponent_onCreditChanged;
        UpdateCredit(creditComponent.Credits);
    }

    private void CreditComponent_onCreditChanged(int newCredits)
    {
        UpdateCredit(newCredits);
    }

    private void UpdateCredit(int newCredits)
    {
        creditText.text = $"${newCredits}";
        RefreshItems(newCredits);
    }

    private void RefreshItems(int credit)
    {
        foreach(var itemUI in shopItems)
        {
            itemUI.Refresh(credit);
        }
    }

    private void InitShopItems()
    {
        ShopItem[] items = shopSystem.GetShopItems();
        foreach(var item in  items)
        {
            AddShopItemUI(item);
        }
    }

    private void AddShopItemUI(ShopItem item)
    {
        ShopItemUI itemUI = Instantiate(shopItemUIPrefab, shopList);
        itemUI.Init(item, creditComponent.Credits);
        itemUI.onItemSelected += ItemUI_onItemSelected;

        shopItems.Add(itemUI);
    }

    private void ItemUI_onItemSelected(ShopItemUI selectedItem)
    {
        this.selectedShopItemUI = selectedItem;
    }

    private void TryPurchaseItem()
    {
        if (selectedShopItemUI == null ||
            !shopSystem.TryPurchase(selectedShopItemUI.GetShopItem(), creditComponent))
        {
            return;
        }

        RemoveShopItemUI(selectedShopItemUI);
        selectedShopItemUI = null;
    }

    private void RemoveShopItemUI(ShopItemUI shopItemToRemove)
    {
        shopItems.Remove(shopItemToRemove);
        Destroy(shopItemToRemove.gameObject);
    }
}
