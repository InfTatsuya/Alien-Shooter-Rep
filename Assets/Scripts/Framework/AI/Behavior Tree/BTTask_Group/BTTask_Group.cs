using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BTTask_Group : BTNode
{
    private BTNode root;
    protected BehaviorTree behaviorTree;

    public BTTask_Group(BehaviorTree behaviourTree)
    {
        this.behaviorTree = behaviourTree;
    }

    protected abstract void ConstructTree(out BTNode root);

    protected override NodeResult Execute()
    {
        return NodeResult.InProgress;
    }

    protected override NodeResult Update()
    {
        return root.UpdateNode();
    }

    protected override void End()
    {
        root.Abort();
        base.End();
    }

    public override void SortPriority(ref int priorityCounter)
    {
        base.SortPriority(ref priorityCounter);

        root.SortPriority(ref priorityCounter);
    }

    public override void Initialize()
    {
        base.Initialize();

        ConstructTree(out root);
    }

    public override BTNode Get()
    {
        return root.Get();
    }
}
