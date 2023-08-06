using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityComponent : MonoBehaviour, IPurchaseListener, IRewardListener
{
    public delegate void OnNewAbilityAdded(Ability newAbility);
    public event OnNewAbilityAdded onNewAbilityAdded;

    public delegate void OnStaminaChange(float newValue, float maxStamina);
    public event OnStaminaChange onStaminaChange;

    [SerializeField] Ability[] initialAbilities;

    [SerializeField] float stamina = 100f;
    [SerializeField] float maxStamina = 100f;

    private List<Ability> abilities = new List<Ability>();

    private void Start()
    {
        foreach(var ability in initialAbilities)
        {
            GiveAbility(ability);
        }
    }

    private void GiveAbility(Ability ability)
    {
        Ability newAbility = Instantiate(ability);
        newAbility.InitAbility(this);
        abilities.Add(newAbility);

        onNewAbilityAdded?.Invoke(newAbility);
    }

    public void ActivateAbility(Ability abilityToActive)
    {
        if(abilities.Contains(abilityToActive))
        {
            abilityToActive.ActivateAbility();
        }
    }

    public float GetStamina() => stamina;

    public bool TryConsumeStamina(float amt)
    {
        if(stamina < amt) return false;

        stamina -= amt;
        BroadcastStaminaChangeImmediately();
        return true;
    }

    public void BroadcastStaminaChangeImmediately()
    {
        onStaminaChange?.Invoke(stamina, maxStamina);
    }

    public bool HandlePurchase(Object newPurchase)
    {
        Ability itemAsAbility = newPurchase as Ability;

        if (itemAsAbility == null) return false;

        GiveAbility(itemAsAbility);
        return true;
    }

    public void Reward(Reward reward)
    {
        stamina = Mathf.Clamp(stamina + reward.staminaReward, 0f, maxStamina);
        BroadcastStaminaChangeImmediately();
    }
}
