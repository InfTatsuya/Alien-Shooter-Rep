using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Compositor : BTNode
{
    LinkedList<BTNode> children = new LinkedList<BTNode>();
    LinkedListNode<BTNode> currentChild = null;

    protected override NodeResult Execute()
    {
        if(children.Count == 0)
        {
            return NodeResult.Success;
        }

        currentChild = children.First;
        return NodeResult.InProgress;
    }

    protected bool Next()
    {
        if(currentChild != children.Last)
        {
            currentChild = currentChild.Next;
            return true;
        }
 
        return false;
    }

    protected override void End()
    {
        if (currentChild == null) return;

        currentChild.Value.Abort();
        currentChild = null;
    }

    protected BTNode GetCurrentChild() => currentChild.Value;

    public void AddChild(BTNode child)
    {
        children.AddLast(child);
    }

    public override void SortPriority(ref int priorityCounter)
    {
        base.SortPriority(ref priorityCounter);

        foreach(BTNode child in children)
        {
            child.SortPriority(ref priorityCounter);    
        }
    }

    public override void Initialize()
    {
        base.Initialize();

        foreach (BTNode child in children)
        {
            child.Initialize();
        }
    }

    public override BTNode Get()
    {
        if(currentChild == null)
        {
            if(children.Count > 0)
            {
                return children.First.Value.Get();
            }
            else
            {
                return this;
            }
        }

        return currentChild.Value.Get();
    }

}
