using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownDecorator : Decorator
{
    private float cooldownTime;
    private float lastExecuteTime = -1.1f;
    private bool failureOnCooldown;

    public CooldownDecorator(BehaviorTree tree, BTNode child, float cooldownTime, bool failureOnCooldown = false) : base(tree, child)
    {
        this.cooldownTime = cooldownTime;
        this.failureOnCooldown = failureOnCooldown;
    }

    protected override NodeResult Execute()
    {
        if (cooldownTime <= 0f) return NodeResult.InProgress;

        if(lastExecuteTime <= -1f)
        {
            lastExecuteTime = Time.timeSinceLevelLoad;
            return NodeResult.InProgress;
        }

        if(Time.timeSinceLevelLoad - lastExecuteTime < cooldownTime)
        {
            if(failureOnCooldown)
            {
                return NodeResult.Failure;
            }
            else
            {
                return NodeResult.Success;
            }
        }

        lastExecuteTime = Time.timeSinceLevelLoad;
        return NodeResult.InProgress;
    }

    protected override NodeResult Update()
    {
        return Child.UpdateNode();
    }

    protected override void End()
    {
        base.End();
    }
}
