using Cysharp.Threading.Tasks;
using Pathfinding;
using System.Collections;
using UnityEngine;

public class Pet : MonoBehaviour
{
    public PetType PetType;
    public Customer Owner;

    private PetZone _currentZone = null;
    public PetZone CurrentZone
    {
        set { _currentZone = value; }
    }

    private Animator _animator;

    private FollowerEntity followerEntity;
    [SerializeField] AIDestinationSetter destinationSetter;
    Coroutine patrolCoroutine;
    private void Start()
    {
        followerEntity = GetComponent<FollowerEntity>();
        destinationSetter = GetComponent<AIDestinationSetter>();
        _animator = GetComponentInChildren<Animator>();
    }
    public void FollowOwner()
    {
        if(patrolCoroutine != null)
            StopCoroutine(patrolCoroutine);
        followerEntity.enabled = true;
        destinationSetter.target = Owner.PetHolder;
    }
    public void Patrol()
    {
        followerEntity.enabled = true;
        destinationSetter.target = null;
        patrolCoroutine = StartCoroutine(PatrolCoroutine());
    }
    public void StopPatrolling()
    {
        followerEntity.enabled = false;
        if(patrolCoroutine != null)
            StopCoroutine(patrolCoroutine);
    }
    private IEnumerator PatrolCoroutine()
    {
        var destination1 = _currentZone.RandomPatrolPosition();
        UniTask task = SetTarget(destination1);
        yield return new WaitUntil(() => task.Status.IsCompleted());
        yield return new WaitForSeconds(3f);
        Patrol();
        yield return null;
    }
    private void Update()
    {
        if(followerEntity.enabled) 
            _animator.SetFloat("Speed", followerEntity.velocity.magnitude);
    }
    public virtual async UniTask SetTarget(Vector3 target)
    {
        followerEntity.destination = target;
        await UniTask.WaitUntil(() => followerEntity.reachedDestination || followerEntity.reachedEndOfPath);
    }
}
