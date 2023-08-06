using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryComponent : MonoBehaviour, IPurchaseListener
{
    [SerializeField] Weapon[] initialWeaponPrefabs;

    [SerializeField] Transform defaultWeaponSlot;
    [SerializeField] Transform[] weaponSlots;

    private List<Weapon> weapons;

    public Weapon CurrentWeapon => weapons[currentWeaponIndex];

    private int currentWeaponIndex = -1;

    private void Start()
    {
        weapons = new List<Weapon>();

        InitializeWeapon();
    }

    private void InitializeWeapon()
    {
        foreach(Weapon weapon in initialWeaponPrefabs)
        {
            AddNewWeapon(weapon);
        }

        NextWeapon();
    }

    private void AddNewWeapon(Weapon weapon)
    {
        Transform weaponSlot = defaultWeaponSlot;
        foreach (Transform slot in weaponSlots)
        {
            if (slot.gameObject.CompareTag(weapon.AttachSlotTag))
            {
                weaponSlot = slot;
            }
        }

        Weapon newWeapon = Instantiate(weapon, weaponSlot);
        newWeapon.transform.localPosition = Vector3.zero;
        newWeapon.Init(this.gameObject);

        weapons.Add(newWeapon);
    }

    public void NextWeapon()
    {
        int nextWeaponIndex = currentWeaponIndex + 1;
        if(nextWeaponIndex >= weapons.Count)
        {
            nextWeaponIndex = 0;
        }

        EquipWeapon(nextWeaponIndex);
    }

    private void EquipWeapon(int weaponIndex)
    {
        if (weaponIndex < 0 || weaponIndex >= weapons.Count) return;

        if(currentWeaponIndex >= 0 && currentWeaponIndex < weapons.Count)
        {
            weapons[currentWeaponIndex].Unequip();
        }

        weapons[weaponIndex].Equip();
        currentWeaponIndex = weaponIndex;
    }

    public bool HandlePurchase(UnityEngine.Object newPurchase)
    {
        GameObject itemAsGameObject = newPurchase as GameObject;

        if (itemAsGameObject == null) return false;

        Weapon newWeapon = itemAsGameObject.GetComponent<Weapon>();
        if (newWeapon == null) return false;

        bool hasWeapon = true;
        if(weapons.Count <= 0)
        {
            hasWeapon = false;
        }

        AddNewWeapon(newWeapon);

        if(!hasWeapon)
        {
            EquipWeapon(0);
        }

        return true;
    }
}
