using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightSense : SenseComponent
{
    [SerializeField] float sightDistance = 8f;
    [SerializeField] float sightHalfAngle = 30f;
    [SerializeField] float eyeHeight = 1f;

    protected override bool InStimuliSensable(PerceptionStimulus stimulus)
    {
        float distance = Vector3.Distance(stimulus.transform.position, transform.position);
        if (distance > sightDistance) return false;

        Vector3 forwardDir = transform.forward;
        Vector3 stimulusDir = (stimulus.transform.position - transform.position).normalized;
        if (Vector3.Dot(forwardDir, stimulusDir) < Mathf.Cos(sightHalfAngle * Mathf.Deg2Rad)) 
            return false;

        if(Physics.Raycast( transform.position + Vector3.up * eyeHeight, 
                            stimulusDir, out RaycastHit hit, 
                            sightDistance))
        {
            if(hit.collider.gameObject != stimulus.gameObject) 
                return false;
        }

        return true;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Vector3 drawCenter = transform.position + Vector3.up * eyeHeight;
        Gizmos.DrawWireSphere(drawCenter, sightDistance);

        Vector3 leftLimitDir = Quaternion.AngleAxis(-sightHalfAngle, Vector3.up) * transform.forward;
        Vector3 rightLimitDir = Quaternion.AngleAxis(sightHalfAngle, Vector3.up) * transform.forward;

        Gizmos.DrawLine(drawCenter, drawCenter + leftLimitDir * sightDistance);
        Gizmos.DrawLine(drawCenter, drawCenter + rightLimitDir * sightDistance);
    }
}
