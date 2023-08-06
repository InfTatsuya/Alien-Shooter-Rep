using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Ability/Health Regen")]
public class Ability_HealthRegen : Ability
{
    [SerializeField] float healthRegenAmt = 50f;
    [SerializeField] float healthRegenDuration = 3f;

    public override void ActivateAbility()
    {
        if(!CommitAbility()) return;

        HealthComponent healthComp = AbilityComp.GetComponent<HealthComponent>();
        if(healthComp != null)
        {
            if (healthRegenDuration == 0)
            {
                healthComp.ChangeHealth(healthRegenAmt, healthComp.gameObject);
                return;
            }

            AbilityComp.StartCoroutine(StartHealthRegen(healthRegenAmt, healthRegenDuration, healthComp));
        }
    }

    private IEnumerator StartHealthRegen(float amt, float duration, HealthComponent healthComp)
    {
        float timer = duration;
        float regenRate = amt / duration;
        while(timer > 0)
        {
            timer -= Time.deltaTime;
            healthComp.ChangeHealth(regenRate * Time.deltaTime, healthComp.gameObject);

            yield return null;
        }
    }
}
