using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTTask_Wait : BTNode
{
    float waitTime = 2f;
    float waitTimer;


    public BTTask_Wait(float waitTime)
    {
        this.waitTime = waitTime;
    }

    protected override NodeResult Execute()
    {
        if(waitTime < 0f)
        {
            return NodeResult.Success;
        }

        waitTimer = 0f;
        return NodeResult.InProgress;
    }

    protected override NodeResult Update()
    {
        waitTimer += Time.deltaTime;
        if(waitTimer > waitTime)
        {
            return NodeResult.Success;
        }

        return NodeResult.InProgress;
    }
}
