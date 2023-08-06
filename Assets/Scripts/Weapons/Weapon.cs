using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    private readonly int speedString = Animator.StringToHash("animationSpeed");

    [SerializeField] string attachSlotTag;
    [SerializeField] AnimatorOverrideController overrideController;
    [SerializeField] float attackRateMultiplier = 1f;

    
    public string AttachSlotTag => attachSlotTag;

    public GameObject Owner { get; private set; }

    public abstract void Attack();

    public virtual void Init(GameObject owner)
    { 
        Owner = owner;
        Unequip();
    }

    public void Equip()
    {
        gameObject.SetActive(true);

        Animator anim = Owner.GetComponent<Animator>();
        anim.runtimeAnimatorController = overrideController;
        anim.SetFloat(speedString, attackRateMultiplier);
    }

    public void Unequip()
    {
        gameObject.SetActive(false);
    }

    public void DealDamage(GameObject objectToDamage, float damageAmt)
    {
        if (objectToDamage == Owner) return;

        if(objectToDamage.TryGetComponent<HealthComponent>(out var health))
        {
            health.ChangeHealth(-damageAmt, Owner);
        }
    }
}
