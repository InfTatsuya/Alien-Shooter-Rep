using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeResult
{
    Success,
    Failure,
    InProgress
}


public abstract class BTNode 
{
    private bool started = false;
    private int priority;
    public int Priority => priority;


    public NodeResult UpdateNode()
    {
        if (!started)
        {
            started = true;
            NodeResult executeResult = Execute();

            if(executeResult != NodeResult.InProgress )
            {
                EndNode();
                return executeResult;
            }
        }

        NodeResult updateResult = Update();
        if(updateResult != NodeResult.InProgress)
        {
            EndNode();
        }

        return updateResult;
    }

    protected virtual NodeResult Update()
    {
        return NodeResult.Success;
    }

    protected virtual NodeResult Execute()
    {
        return NodeResult.Success;
    }

    protected virtual void End()
    {

    }

    private void EndNode()
    {
        started = false;
        End();
    }

    public void Abort()
    {
        EndNode();
    }

    public virtual void SortPriority(ref int priorityCounter)
    {
        priority = priorityCounter++;
        Debug.Log($"{this} has priority: {priority}");
    }

    public virtual void Initialize()
    {

    }

    public virtual BTNode Get() => this;

}
