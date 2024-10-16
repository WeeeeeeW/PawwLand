using Cysharp.Threading.Tasks;
using DG.Tweening;
using Pathfinding;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Entity : SerializedMonoBehaviour
{
    protected FollowerEntity agent;
    [SerializeField] protected Transform petHolder;
    protected Pet currentPet;
    private void Awake()
    {
        agent = GetComponent<FollowerEntity>();
    }
    public virtual async UniTask SetTarget(Transform target)
    {
        agent.updateRotation = true;
        agent.destination = target.position;
        await UniTask.WaitUntil(() => agent.reachedDestination);
    }
    public virtual async UniTask SetTarget(Transform target, Quaternion rotation)
    {
        agent.updateRotation = true;
        agent.destination = target.position;
        await UniTask.WaitUntil(() => agent.reachedDestination);
        await transform.DORotateQuaternion(rotation, .2f);
    }
    public virtual async UniTask SetTarget(Transform[] targets)
    {
        foreach (Transform target in targets)
        {
            await SetTarget(target);
        }
    }
    public virtual async UniTask SetTarget(Vector3 target)
    {
        agent.updateRotation = true;
        agent.destination = target;
        await UniTask.WaitUntil(() => agent.reachedDestination);
    }
    public virtual async UniTask SetTarget(Vector3 target, Quaternion rotation)
    {
        agent.updateRotation = true;
        agent.destination = target;
        await UniTask.WaitUntil(() => agent.reachedDestination);
        await transform.DORotateQuaternion(rotation, .2f);
    }
    public virtual async UniTask SetTarget(Vector3[] targets)
    {
        foreach (Vector3 target in targets)
        {
            await SetTarget(target);
        }
    }
    public virtual IEnumerator Patrol()
    {
        agent.updateRotation = true;
        yield return new WaitForSeconds(3f);
        GraphNode randomNode;

        // For grid graphs
        var grid = AstarPath.active.data.gridGraph;
        randomNode = grid.nodes[Random.Range(0, grid.nodes.Length)];
        Debug.Log(randomNode);

        // Use the center of the node as the destination for example
        var destination1 = (Vector3)randomNode.position;
        agent.SetDestination(destination1);
    }
    public async UniTask SetQueueTarget(Vector3 _target)
    {
        agent.updateRotation = true;
        agent.SetDestination(_target);
        await UniTask.WaitUntil(() => agent.reachedDestination);
    }


    public void PickupPet(Pet pet)
    {
        currentPet = pet;
        currentPet.transform.parent = petHolder;
        currentPet.transform.localPosition = Vector3.zero;
    }
    public Pet DropOffPet()
    {
        currentPet.transform.parent = null;
        var temp = currentPet;
        currentPet = null;
        return temp;
    }
}
