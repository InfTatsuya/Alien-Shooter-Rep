using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour, IRewardListener
{
    public delegate void OnHealthChange(float currentHealth, float delta, float maxHealth);
    public event OnHealthChange onHealthChanged;

    public delegate void OnTakeDamage(float currentHealth, float delta, float maxHealth, GameObject instigator);
    public event OnTakeDamage onTakeDamage;

    public delegate void OnHealthEmpty(GameObject killer);
    public event OnHealthEmpty onHealthEmpty;

    [SerializeField] float health = 100f;
    [SerializeField] float maxHealth = 100f;

    public void BoardcastHealthValueImmediately()
    {
        onHealthChanged?.Invoke(maxHealth, 0, maxHealth);
    }

    public void ChangeHealth(float amt, GameObject instigator)
    {
        if(amt == 0 || health <= 0f) return;

        health += amt;
        health = Mathf.Clamp(health, -1f, maxHealth);

        if(amt < 0f)
        {
            onTakeDamage?.Invoke(health, amt, maxHealth, instigator);
        }

        onHealthChanged?.Invoke(health, amt, maxHealth);

        if(health <= 0f)
        {
            health = -0.1f;
            onHealthEmpty?.Invoke(instigator);
        }

        //Debug.Log($"{gameObject.name} taking damage {amt}, current health: {health}");
    }

    public void Reward(Reward reward)
    {
        ChangeHealth(reward.healthReward, null);
    }
}
