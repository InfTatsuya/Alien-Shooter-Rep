using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviorTree : MonoBehaviour
{
    private BTNode root;
    private Blackboard blackboard = new Blackboard();
    public Blackboard BlackBoard { get => blackboard; }
    
    private IBehaviorTreeInterface behaviorTreeInterface;
    public IBehaviorTreeInterface BehaviorTreeInterface => behaviorTreeInterface;

    private void Start()
    {
        behaviorTreeInterface = GetComponent<IBehaviorTreeInterface>();
        ConstructTree(out root);
        SortTree();
    }

    private void Update()
    {
        root.UpdateNode();
    }

    protected abstract void ConstructTree(out BTNode rootNode);

    private void SortTree()
    {
        int priorityCounter = 0;
        root.Initialize();
        root.SortPriority(ref priorityCounter);
    }

    public void AbortLowerThan(int priority)
    {
        BTNode currentNode = root.Get();
        if(currentNode.Priority > priority)
        {
            root.Abort();
        }
    }

}
