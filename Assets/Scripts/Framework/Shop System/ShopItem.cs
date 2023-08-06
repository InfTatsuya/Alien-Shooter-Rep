using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Shop/Shop Item", fileName ="ShopItem_")]
public class ShopItem : ScriptableObject
{
    public string title;
    public int price;
    public Object item;
    public Sprite itemIcon;
    [TextArea] public string description;
}
