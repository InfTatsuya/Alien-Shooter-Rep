using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spitter : Enemy
{
    [SerializeField] Projectile projectilePrefab;
    [SerializeField] Transform launchPoint;

    private Vector3 destination;

    public override void AttackTarget(GameObject target)
    {
        Anim.SetTrigger(StringCollector.attackAnim);

        destination = target.transform.position;
    }

    private void Shoot()
    {
        Projectile newProjectile = Instantiate(projectilePrefab, launchPoint.position, launchPoint.rotation);
        newProjectile.Launch(this.gameObject, destination);
    }
}
