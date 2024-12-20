using Cysharp.Threading.Tasks;
using Pathfinding;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

public class Pet : MonoBehaviour
{
    public PetType PetType;
    public Customer Owner;
    [SerializeField] GameObject _petWaste;

    [SerializeField] private PetZone _currentZone = null;
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
        if (patrolCoroutine != null)
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
        destinationSetter.target = null;
        followerEntity.enabled = false;
        _animator.SetFloat("Speed", 0);
        if (patrolCoroutine != null)
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
        if (followerEntity.enabled)
            _animator.SetFloat("Speed", followerEntity.velocity.magnitude);
    }
    public virtual async UniTask SetTarget(Vector3 target)
    {
        followerEntity.destination = target;
        await UniTask.WaitUntil(() => followerEntity.reachedDestination || followerEntity.reachedEndOfPath);
    }

    [Button]
    void Test()
    {
        PetWaste waste = Instantiate(_petWaste, new Vector3(transform.position.x, 0, transform.position.z), transform.rotation).GetComponent<PetWaste>();
        waste.CreateWaste(_currentZone.EmployeePickupPoint);
    }
}
