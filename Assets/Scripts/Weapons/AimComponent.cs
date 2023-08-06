using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimComponent : MonoBehaviour
{
    [SerializeField] Transform muzzle;
    [SerializeField] float shootingRange= 1000f;
    [SerializeField] LayerMask aimMask;

    public GameObject GetAimTarget(out Vector3 aimDirection)
    {
        Vector3 aimStart = muzzle.position;
        aimDirection = GetAimDirection();

        RaycastHit hit;
        if(Physics.Raycast(aimStart, aimDirection, out hit, shootingRange, aimMask))
        {
            return hit.collider.gameObject;
        }

        return null;
    }

    private Vector3 GetAimDirection()
    {
        Vector3 muzzleDir = muzzle.forward;
        return new Vector3(muzzleDir.x, 0f, muzzleDir.z).normalized;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(muzzle.position, muzzle.position + GetAimDirection() * shootingRange);
    }
}
