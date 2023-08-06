using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Decorator : BTNode
{
    protected BehaviorTree behaviorTree;

    private BTNode child;
    protected BTNode Child => child;

    public Decorator(BehaviorTree tree, BTNode child)
    {
        this.behaviorTree = tree;
        this.child = child;
    }

    public override void SortPriority(ref int priorityCounter)
    {
        base.SortPriority(ref priorityCounter);
        child.SortPriority(ref priorityCounter);
    }

    public override void Initialize()
    {
        base.Initialize();

        child.Initialize();
    }

    public override BTNode Get()
    {
        return child.Get();
    }
}
