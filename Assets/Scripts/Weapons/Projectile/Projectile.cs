using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float flightHeight;
    [SerializeField] DamageComponent damageComponent;
    [SerializeField] ParticleSystem explodeVFX;

    private Rigidbody rb;

    private ITeamInterface instigatorTeamId;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Launch(GameObject instigator, Vector3 destination)
    {
        instigatorTeamId = instigator.GetComponent<ITeamInterface>();
        if(instigatorTeamId != null )
        {
            damageComponent.SetupTeamInterface(instigator);
        }

        float gravity = Physics.gravity.magnitude;
        float halfTime = Mathf.Sqrt((2 * flightHeight) / gravity);

        float upSpeed = halfTime * gravity;

        Vector3 targetDir = destination - transform.position;
        targetDir.y = 0f;
        float horizontalDist = targetDir.magnitude;
        float horizontalSpeed = horizontalDist / (2f * halfTime);

        Vector3 velocity = Vector3.up * upSpeed + targetDir.normalized * horizontalSpeed;

        rb.AddForce(velocity, ForceMode.VelocityChange);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(instigatorTeamId.GetRelationToward(other.gameObject) != TeamRelation.Friendly)
        {
            Explode();
        }
    }

    private void Explode()
    {
        Instantiate(explodeVFX, transform.position, Quaternion.identity);

        Destroy(this.gameObject);
    }
}
