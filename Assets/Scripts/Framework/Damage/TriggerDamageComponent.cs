using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDamageComponent : DamageComponent
{
    [SerializeField] float damage;
    [SerializeField] bool startedEnabled = false;

    private BoxCollider trigger;
    
    public void SetDamageEnabled(bool enabled)
    {
        trigger.enabled = enabled;
    }

    private void Start()
    {
        trigger = GetComponent<BoxCollider>();
        SetDamageEnabled(startedEnabled);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!ShouldDamage(other.gameObject)) return;

        if(other.TryGetComponent<HealthComponent>(out var health))
        {
            health.ChangeHealth(-damage, this.gameObject);
        }
    }
}
