using Cysharp.Threading.Tasks;
using Pathfinding;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class Customer : Entity
{
    public string customerName;
    public ServiceType requestedService;
    public Pet pet;
    private TaskManager taskManager;
    void Start()
    {
        // Assign the task manager
        taskManager = TaskManager.Instance;
        navMeshAgent = GetComponent<FollowerEntity>();
        pet.AssignToOwner(this);
        MoveToCounter();
    }

    private async void MoveToCounter()
    {
        // Request a task based on service type
        assignedStation = TaskManager.Instance.counter;
        Debug.Log($"{customerName} is coming in with {pet.petName}.");
        await assignedStation.QueueUp(this);
    }

    public async void RequestService()
    {
        await UniTask.WaitForSeconds(assignedStation.taskDuration);
        DropOffPet();
        assignedStation.AdvanceQueue();
        await SetTarget(assignedStation.exitTF);
        SetTarget(TaskManager.Instance.door);
    }

    private void DropOffPet()
    {
        var manager = ((Counter)assignedStation).manager;
        pet.AssignToEntity(manager);
        manager.AssignTask(requestedService, pet);
    }

    public void ProceedPayment()
    {
        gameObject.SetActive(true);

    }
    void Pay()
    {
        //Tip logic here also
        //Debug.Log($"{customerName} paid <color=green>$xxx</color>");
        //UnsubscribeToCounter(assignedCounter);
        //SetTarget(taskManager.petzone.customerDoor);
    }

    IEnumerator PickupPet()
    {
        yield return new WaitForSeconds(.2f);
        pet.transform.parent = petHolder;
        pet.transform.localPosition = Vector3.zero;
        SetTarget(TaskManager.Instance.door);
    }

    public void Leave()
    {
        // Customer leaves after service is done
        Debug.Log($"{customerName} is leaving the spa.");
        Destroy(gameObject);  // For now, we'll just destroy the customer object.
    }

    Action _actionRef;
    public void SubscribeToCounter(Counter _counter)
    {
        _counter.advanceQueue += _actionRef;
    }
    public void UnsubscribeToCounter(Counter _counter)
    {
        _counter.advanceQueue -= _actionRef;
    }
}