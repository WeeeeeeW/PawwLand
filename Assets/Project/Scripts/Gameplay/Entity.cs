using Pathfinding;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

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

    public virtual IEnumerator Patrol()
    {
        navMeshAgent.updateRotation = true;
        yield return new WaitForSeconds(3f);
        GraphNode randomNode;

        // For grid graphs
        var grid = AstarPath.active.data.gridGraph;
        randomNode = grid.nodes[Random.Range(0, grid.nodes.Length)];
        Debug.Log(randomNode);

        // Use the center of the node as the destination for example
        var destination1 = (Vector3)randomNode.position;
        navMeshAgent.SetDestination(destination1);
    }

    public void AddActionQueue(Action _action)
    {
        actionQueue.Enqueue(_action);
    }
    public void InvokeQueue()
    {
        actionQueue.Dequeue().Invoke();
    }

    public void SetQueueTarget(Vector3 _target)
    {
        destination = null;
        navMeshAgent.updateRotation = true;
        navMeshAgent.SetDestination(_target);
    }
    public void SetQueueTarget(Transform _target)
    {
        SetTarget(_target);
    }
}
