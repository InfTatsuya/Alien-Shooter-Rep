using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Ability/Fire")]
public class Ability_Fire : Ability
{
    [SerializeField] Scanner scannerPrefab;
    [SerializeField] float fireRadius = 10f;
    [SerializeField] float fireDuration = 0.5f;
    [SerializeField] float damageDuration = 3f;
    [SerializeField] float fireDamage = 60f;

    [SerializeField] GameObject scanVFX;
    [SerializeField] GameObject damageVFX;

    public override void ActivateAbility()
    {
        if(!CommitAbility()) return;

        Scanner fireScanner = Instantiate(scannerPrefab, AbilityComp.transform);
        fireScanner.SetScanRange(fireRadius);
        fireScanner.SetScanDuration(fireDuration);
        fireScanner.AddChildAttached(Instantiate(scanVFX).transform);
        fireScanner.onScanDetectionUpdated += FireScanner_onScanDetectionUpdated;
        fireScanner.StartScan();
    }

    private void FireScanner_onScanDetectionUpdated(GameObject newDetection)
    {
        ITeamInterface detectedTeamInterface = newDetection.GetComponent<ITeamInterface>();
        if( detectedTeamInterface == null || 
            detectedTeamInterface.GetRelationToward(AbilityComp.gameObject) != TeamRelation.Enemy)
        {
            return;
        }

        HealthComponent enemyHealth = newDetection.GetComponent<HealthComponent>();
        if(enemyHealth == null)
        {
            return;
        }

        AbilityComp.StartCoroutine(ApplyDamageTo(enemyHealth));
    }

    private IEnumerator ApplyDamageTo(HealthComponent health)
    {
        GameObject damageEffect = Instantiate(damageVFX, health.transform);

        float damageRate = fireDamage / damageDuration;
        float timer = 0f;

        while(timer <= damageDuration && health != null)
        {
            timer += Time.deltaTime;
            health.ChangeHealth(-damageRate * Time.deltaTime, AbilityComp.gameObject);

            yield return null;
        }
        if(damageEffect != null)
        {
            Destroy(damageEffect);
        }
    }
}
