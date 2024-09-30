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

    void Start()
    {
        // Assign the task manager
        taskManager = TaskManager.Instance;
        navMeshAgent = GetComponent<NavMeshAgent>();
        actionQueue = new Queue<Action>();

        MoveToCounter();
    }

    private void MoveToCounter()
    {
        // Request a task based on service type
        //Debug.Log($"{customerName} is coming in with {pet.petName}.");
        SetTarget(TaskManager.Instance.customerCounter);

        actionQueue.Enqueue(() => StartCoroutine(RegisterTask()));
    }

    private IEnumerator RegisterTask()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log($"{customerName} requests {requestedService}");
        TaskManager.Instance.manager.AssignTask(requestedService, pet);
        //taskManager.CreateTask(this, requestedService);
        SetTarget(TaskManager.Instance.door);
        actionQueue.Enqueue(() => WaitPetFinish());
    }

    void WaitPetFinish()
    {

    }

    public void Leave()
    {
        // Customer leaves after service is done
        Debug.Log($"{customerName} is leaving the spa.");
        Destroy(gameObject);  // For now, we'll just destroy the customer object.
    }
}