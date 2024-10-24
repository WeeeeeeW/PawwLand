using Cysharp.Threading.Tasks;
using System.Collections;
using System.Threading;
using UnityEngine;

public class Customer : Entity
{
    public string customerName;
    public ServiceType requestedService;
    Counter counter;
    public Pet pet;
    public bool Paying;
    protected override void Start()
    {
        base.Start();
        pet.owner = this;
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
    public async void Leave(bool reset = false)
    {
        await SetTarget(TaskManager.Instance.door);
        gameObject.SetActive(false);
        if (reset)
            //TODO: Impleement This
            Destroy(gameObject);
    }
    public async void LeaveCounter()
    {
        await SetTarget(new Transform[] { counter.CounterExit(), TaskManager.Instance.door });
        gameObject.SetActive(false);
    }

    public void MakePayement()
    {
        gameObject.SetActive(true);
        Paying = true;
        counter.AssignCustomerToCounter(this);
    }
    public async void PickupPet()
    {
        await SetTarget(taskManager.petZones[pet.petType].CustomerPickupPoint);
        PickupPet(pet);
        Leave(true);
    }

    public override void Patrol()
    {
        throw new System.NotImplementedException();
    }
}
