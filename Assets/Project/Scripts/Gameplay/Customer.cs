using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Customer : Entity
{
    public string customerName;
    public ServiceType requestedService;
    TaskManager taskManager;
    Counter counter;
    public Pet pet;
    private void Start()
    {
        taskManager = TaskManager.Instance;
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
}
