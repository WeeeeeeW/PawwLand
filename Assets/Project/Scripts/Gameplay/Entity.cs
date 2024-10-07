using Pathfinding;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Entity : SerializedMonoBehaviour
{
    protected FollowerEntity navMeshAgent;
    [ReadOnly] public Transform destination;
    [SerializeField] protected Queue<Action> actionQueue;

    protected virtual void SetTarget(Transform target)
    {
        navMeshAgent.updateRotation = true;
        destination = target;
        navMeshAgent.destination = destination.position;
    }
    public virtual void ReachDestination()
    {
        if (actionQueue.Count > 0)
        {
            navMeshAgent.updateRotation = false;
            //navMeshAgent.velocity = Vector3.zero;
            actionQueue.Dequeue().Invoke();
        }
    }

}
