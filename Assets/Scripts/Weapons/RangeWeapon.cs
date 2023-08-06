using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AimComponent))]
public class RangeWeapon : Weapon
{
    [SerializeField] protected float damage = 10f;
    [SerializeField] ParticleSystem bulletVFX;

    protected AimComponent aimComponent;

    public override void Init(GameObject owner)
    {
        base.Init(owner);

        aimComponent = GetComponent<AimComponent>();
    }

    public override void Attack()
    {
        GameObject target = aimComponent.GetAimTarget(out Vector3 aimDir);
        DealDamage(target, damage);

        bulletVFX.transform.rotation = Quaternion.LookRotation(aimDir);
        bulletVFX.Emit(bulletVFX.emission.GetBurst(0).maxCount);
    }
}
