using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBehaviorTreeInterface 
{
    public void RotateToward(GameObject target, bool verticalAim = false);
    public void AttackTarget(GameObject target);
}
