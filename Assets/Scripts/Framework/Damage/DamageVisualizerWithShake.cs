using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageVisualizerWithShake : DamageVisualizer
{
    [SerializeField] Shaker shaker;

    protected override void HealthComponent_onTakeDamage(float currentHealth, float delta, float maxHealth, GameObject instigator)
    {
        base.HealthComponent_onTakeDamage(currentHealth, delta, maxHealth, instigator);

        shaker?.StartShaking();
    }
}
