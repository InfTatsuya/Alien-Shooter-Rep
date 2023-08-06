using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSense : SenseComponent
{
    [SerializeField] float hitForgetTime = 2f;

    private HealthComponent health;

    private Dictionary<PerceptionStimulus, Coroutine> hitRecords = new Dictionary<PerceptionStimulus, Coroutine>();

    protected override bool InStimuliSensable(PerceptionStimulus stimulus)
    {
        return hitRecords.ContainsKey(stimulus);
    }

    private void Start()
    {
        health = GetComponent<HealthComponent>();

        health.onTakeDamage += Health_onTakeDamage;
    }

    private void Health_onTakeDamage(float currentHealth, float delta, float maxHealth, GameObject instigator)
    {
        PerceptionStimulus stimulus = instigator.GetComponent<PerceptionStimulus>();    
        if(stimulus != null)
        {
            Coroutine newForgettingRoutine = StartCoroutine(ForgettingStimulus(stimulus));

            if(hitRecords.TryGetValue(stimulus, out var ongoingCoroutine))
            {
                StopCoroutine(ongoingCoroutine);
                hitRecords[stimulus] = newForgettingRoutine;
            }
            else
            {
                hitRecords.Add(stimulus, newForgettingRoutine);
            }
        }
    }

    private IEnumerator ForgettingStimulus(PerceptionStimulus stimulus)
    {
        yield return new WaitForSeconds(hitForgetTime);

        hitRecords.Remove(stimulus);
    }
}
