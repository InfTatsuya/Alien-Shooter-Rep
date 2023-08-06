using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public delegate void OnCooldownStarted();
    public event OnCooldownStarted onCooldownStarted;

    [SerializeField] Sprite abilityIcon;
    [SerializeField] float staminaCost;
    [SerializeField] float cooldownDuration = 2f;

    public Sprite AbilityIcon => abilityIcon;
    public float CooldownDuration => cooldownDuration;

    private AbilityComponent abilityComponent;
    public AbilityComponent AbilityComp => abilityComponent;

    private bool isAbilityOnCooldown = false;

    public void InitAbility(AbilityComponent abilityComponent)
    {
        this.abilityComponent = abilityComponent;
    }

    public abstract void ActivateAbility();

    protected bool CommitAbility()
    {
        if(isAbilityOnCooldown) return false;

        if(abilityComponent == null || !abilityComponent.TryConsumeStamina(staminaCost))        
            return false;

        StartAbilityCooldown();
        
        return true;
    }

    private void StartAbilityCooldown()
    {
        abilityComponent.StartCoroutine(CooldownRoutine());
    }

    private IEnumerator CooldownRoutine()
    {
        isAbilityOnCooldown = true;
        onCooldownStarted?.Invoke();

        yield return new WaitForSeconds(cooldownDuration);

        isAbilityOnCooldown = false;
    }
}
