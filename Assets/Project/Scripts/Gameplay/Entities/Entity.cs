using Cysharp.Threading.Tasks;
using DG.Tweening;
using Pathfinding;
using Sirenix.OdinInspector;
using System.Collections;
using System.Threading;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    protected FollowerEntity Agent;
    [SerializeField] protected Transform PetHolder;
    protected Pet CurrentPet;
    protected TaskManager TaskManager;
    [SerializeField] protected Animator Animator;
    private void Awake()
    {
        Agent = GetComponent<FollowerEntity>();
    }
    protected virtual void Start()
    {
        TaskManager = TaskManager.Instance;
    }

    protected virtual void Update()
    {
        Animator.SetFloat("Speed", Agent.velocity.magnitude);
    }
    public virtual async UniTask SetTarget(Transform target)
    {
        Agent.destination = target.position;
        await UniTask.WaitUntil(() => Agent.reachedDestination);
    }
    public virtual async UniTask SetTarget(Transform target, Quaternion rotation)
    {
        Agent.destination = target.position;
        await UniTask.WaitUntil(() => Agent.reachedDestination);
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
        Agent.destination = target;
        await UniTask.WaitUntil(() => Agent.reachedDestination || Agent.reachedEndOfPath);
    }
    public virtual async UniTask SetTarget(Vector3 target, Quaternion rotation)
    {
        Agent.destination = target;
        await UniTask.WaitUntil(() => Agent.reachedDestination);
        await transform.DORotateQuaternion(rotation, .2f);
    }
    public virtual async UniTask SetTarget(Vector3[] targets)
    {
        foreach (Vector3 target in targets)
        {
            await SetTarget(target);
        }
    }
    public abstract void Patrol(Transform[] patrolArea);

    public abstract void OnEnterQueue();
    public async UniTask SetQueueTarget(Vector3 _target)
    {
        Agent.SetDestination(_target);
        await UniTask.WaitUntil(() => Agent.reachedDestination);
    }

    public virtual void PickupPet(Pet pet)
    {
        pet.CurrentZone = null;
        pet.StopPatrolling();
        CurrentPet = pet;
        CurrentPet.transform.parent = PetHolder;
        CurrentPet.transform.localPosition = Vector3.zero;
        CurrentPet.transform.rotation = PetHolder.rotation;
        Animator.SetBool("Carry", true);
    }
    public Pet DropOffPet()
    {
        CurrentPet.transform.parent = null;
        var temp = CurrentPet;
        CurrentPet = null;
        Animator.SetBool("Carry", false);
        return temp;
    }
}
