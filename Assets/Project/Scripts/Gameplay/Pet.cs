using Cysharp.Threading.Tasks;
using Pathfinding;
using System.Collections;
using UnityEngine;

public class Pet : MonoBehaviour
{
    public PetType petType;
    public Customer owner;
    public PetZone currentZone = null;
    private FollowerEntity followerEntity;
    private void Start()
    {
        followerEntity = GetComponent<FollowerEntity>();
        followerEntity.enabled = false;
    }
    public void Patrol()
    {
        followerEntity.enabled = true;
        StartCoroutine("PatrolCoroutine");
    }
    public void StopPatrolling()
    {
        followerEntity.enabled = false;
        StopCoroutine("PatrolCoroutine");
    }
    private IEnumerator PatrolCoroutine()
    {
        var destination1 = currentZone.RandomPatrolPosition();
        UniTask task = SetTarget(destination1);
        yield return new WaitUntil(() => task.Status.IsCompleted());
        yield return new WaitForSeconds(3f);
        Patrol();
        yield return null;
    }

    public virtual async UniTask SetTarget(Vector3 target)
    {
        followerEntity.destination = target;
        await UniTask.WaitUntil(() => followerEntity.reachedDestination || followerEntity.reachedEndOfPath);
    }
}
