using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysAwareSense : SenseComponent
{
    [SerializeField] float awareDistance = 5f;

    protected override bool InStimuliSensable(PerceptionStimulus stimulus)
    {
        return Vector3.Distance(transform.position, stimulus.transform.position) < awareDistance;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(transform.position + Vector3.up, awareDistance);
    }
}
