using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Customer : Entity
{
    public string customerName;
    public ServiceType requestedService;
    public Pet pet;
    private TaskManager taskManager;
    [SerializeField] Transform petHolder;
    private Counter assignedCounter;
    void Start()
    {
        // Assign the task manager
        taskManager = TaskManager.Instance;
        navMeshAgent = GetComponent<NavMeshAgent>();
        actionQueue = new Queue<Action>();
        pet.AssignToOwner(this);
        MoveToCounter();
    }

    private void MoveToCounter()
    {
        // Request a task based on service type
        assignedCounter = TaskManager.Instance.counter;
        Debug.Log($"{customerName} is coming in with {pet.petName}.");
        SetTarget(assignedCounter.queueStart);

        actionQueue.Enqueue(() =>
        {
            SubscribeToCounter(assignedCounter);
            assignedCounter.QueueUp(this);
            actionQueue.Enqueue(() => RequestService());
            actionQueue.Enqueue(() => Exit());
        });
        //actionQueue.Enqueue(() => StartCoroutine(RegisterTask()));
    }
    private void RequestService()
    {
        StartCoroutine(assignedCounter.AdvanceQueue(true));
    }
    private void Exit()
    {
        SetTarget(assignedCounter.customerOut);
        actionQueue.Enqueue(() => SetTarget(TaskManager.Instance.door));
        //actionQueue.Enqueue(() => WaitPetFinish());
    }

    public void ProceedPayment()
    {
        gameObject.SetActive(true);
        SetTarget(assignedCounter.queueStart);
        actionQueue.Enqueue(() =>
        {
            SubscribeToCounter(assignedCounter);
            assignedCounter.QueueUp(this);
            actionQueue.Enqueue(() => StartCoroutine(assignedCounter.AdvanceQueue(false)));
            actionQueue.Enqueue(() => Pay());
        });
    }
    void Pay()
    {
        //Tip logic here also
        Debug.Log($"{customerName} paid <color=green>$xxx</color>");
        UnsubscribeToCounter(assignedCounter);
        SetTarget(taskManager.petzone.door);
        actionQueue.Enqueue(() => StartCoroutine(PickupPet()));
    }

    IEnumerator PickupPet()
    {
        yield return new WaitForSeconds(.2f);
        pet.transform.parent = petHolder;
        pet.transform.localPosition = Vector3.zero;
        SetTarget(TaskManager.Instance.door);
        actionQueue.Enqueue(() => Destroy(gameObject));
    }

    public void Leave()
    {
        // Customer leaves after service is done
        Debug.Log($"{customerName} is leaving the spa.");
        Destroy(gameObject);  // For now, we'll just destroy the customer object.
    }

    public void SetQueueTarget(Vector3 _target)
    {
        destination = null;
        navMeshAgent.updateRotation = true;
        navMeshAgent.SetDestination(_target);
    }
    public void SetQueueTarget(Transform _target)
    {
        SetTarget(_target);
    }
    public void AddActionQueue(Action _action)
    {
        actionQueue.Enqueue(_action);
    }
    public void InvokeQueue()
    {
        actionQueue.Dequeue().Invoke();
    }

    Action _actionRef;
    public void SubscribeToCounter(Counter _counter)
    {
        _actionRef = () => actionQueue.Dequeue().Invoke();
        _counter.callNextCustomer += _actionRef;
    }
    public void UnsubscribeToCounter(Counter _counter)
    {
        _counter.callNextCustomer -= _actionRef;
    }
}