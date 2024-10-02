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
        //Debug.Log($"{customerName} is coming in with {pet.petName}.");
        SetTarget(TaskManager.Instance.counter.customerIn);

        actionQueue.Enqueue(() => StartCoroutine(RegisterTask()));
    }

    private IEnumerator RegisterTask()
    {
        yield return new WaitForSeconds(.2f);
        Debug.Log($"{customerName} requests {requestedService}");
        taskManager.manager.AssignTask(requestedService, pet);
        SetTarget(TaskManager.Instance.counter.customerOut);
        actionQueue.Enqueue(() => SetTarget(TaskManager.Instance.door));
        //actionQueue.Enqueue(() => WaitPetFinish());
    }

    public void ProceedPayment()
    {
        SetTarget(taskManager.counter.customerIn);
        actionQueue.Enqueue(() => StartCoroutine(Pay()));
    }

    IEnumerator Pay()
    {
        yield return new WaitForSeconds(.2f);
        //Tip logic here also
        Debug.Log($"{customerName} paid <color=green>$xxx</color>");
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
}