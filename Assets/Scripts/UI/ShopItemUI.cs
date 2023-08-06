using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
    public delegate void OnItemSelected(ShopItemUI selectedItem);
    public event OnItemSelected onItemSelected;

    [SerializeField] Image itemIcon;
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI priceText;
    [SerializeField] TextMeshProUGUI descriptionText;

    [SerializeField] Button selectedButton;
    [SerializeField] Image grayOutImage;

    [SerializeField] Color insufficientCreditColor;
    [SerializeField] Color sufficientCreditColor;

    private ShopItem shopItem;
    public ShopItem GetShopItem() => shopItem;

    private void Start()
    {
        selectedButton.onClick.AddListener(ItemSelected);
    }

    private void ItemSelected()
    {
        onItemSelected?.Invoke(this);
    }

    public void Init(ShopItem item, int availableCredits)
    {
        shopItem = item;

        itemIcon.sprite = item.itemIcon;
        titleText.text = item.title;
        priceText.text = $"${item.price}";
        descriptionText.text = item.description;

        Refresh(availableCredits);
    }

    public void Refresh(int availableCredit)
    {
        if(availableCredit < shopItem.price)
        {
            grayOutImage.enabled = true;
            priceText.color = insufficientCreditColor;
        }
        else
        {
            grayOutImage.enabled = false;
            priceText.color = sufficientCreditColor;
        }
    }
}
