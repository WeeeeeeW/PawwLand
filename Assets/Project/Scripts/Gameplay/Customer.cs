using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class Customer : Entity
{
    public string customerName;
    public ServiceType requestedService;
    Counter counter;
    public Pet pet;
    protected override void Start()
    {
        base.Start();
        currentPet = pet;
        counter = taskManager.counters[0];
        counter.AssignCustomerToCounter(this);
    }

    public void RequestServiceAtCounter()
    {
        // Logic for requesting a service from TaskManager
        Debug.Log($"Customer requests {requestedService} service.");
        DropOffPet();
        LeaveCounter();
    }

    public void ReturnToPlayground()
    {
        // Logic for returning to the playground to pick up the pet
    }
    public async void Leave()
    {
        await SetTarget(TaskManager.Instance.door);
        gameObject.SetActive(false);
    }
    public async void LeaveCounter()
    {
        await SetTarget(new Transform[] { counter.CounterExit(), TaskManager.Instance.door });
        gameObject.SetActive(false);
    }

    public override UniTask Patrol(CancellationToken cancellationToken)
    {
        throw new System.NotImplementedException();
    }
}
