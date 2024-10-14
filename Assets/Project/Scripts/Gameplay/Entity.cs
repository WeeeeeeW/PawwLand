using Cysharp.Threading.Tasks;
using Pathfinding;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Entity : SerializedMonoBehaviour
{
    protected FollowerEntity navMeshAgent;
    protected BaseTaskStation assignedStation;
    [SerializeField] protected Transform petHolder;
    public virtual async UniTask SetTarget(Transform target)
    {
        navMeshAgent.updateRotation = true;
        navMeshAgent.destination = target.position;
        await UniTask.WaitUntil(() => navMeshAgent.reachedDestination);
    }
    public virtual async UniTask SetTarget(Transform[] targets)
    {
        foreach (Transform target in targets)
        {
            navMeshAgent.destination = target.position;
            await UniTask.WaitUntil(() => navMeshAgent.reachedDestination);
        }
    }

    public void AssignPet(Pet _pet)
    {
        _pet.transform.parent = petHolder;
        _pet.transform.localPosition = Vector3.zero;
    }

    public void AssignToStation(BaseTaskStation _station)
    {
        assignedStation = _station;
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
    public void SetQueueTarget(Vector3 _target)
    {
        navMeshAgent.updateRotation = true;
        navMeshAgent.SetDestination(_target);
    }
    public void ExitStation()
    {
        SetTarget(assignedStation.exitTF);
        AssignToStation(null);
    }
}
