using Cysharp.Threading.Tasks;
using DG.Tweening;
using Pathfinding;
using Sirenix.OdinInspector;
using System.Collections;
using System.Threading;
using UnityEngine;

public abstract class Entity : SerializedMonoBehaviour
{
    protected FollowerEntity agent;
    [SerializeField] protected Transform petHolder;
    protected Pet currentPet;
    protected TaskManager taskManager;
    private void Awake()
    {
        agent = GetComponent<FollowerEntity>();
    }
    protected virtual void Start()
    {
        taskManager = TaskManager.Instance;
    }
    public virtual async UniTask SetTarget(Transform target)
    {
        agent.destination = target.position;
        await UniTask.WaitUntil(() => agent.reachedDestination);
    }
    public virtual async UniTask SetTarget(Transform target, Quaternion rotation)
    {
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
        agent.destination = target;
        await UniTask.WaitUntil(() => agent.reachedDestination || agent.reachedEndOfPath);
    }
    public virtual async UniTask SetTarget(Vector3 target, Quaternion rotation)
    {
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
    public abstract void Patrol();
    public async UniTask SetQueueTarget(Vector3 _target)
    {
        agent.SetDestination(_target);
        await UniTask.WaitUntil(() => agent.reachedDestination);
    }


    public virtual void PickupPet(Pet pet)
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
