using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour
{
    public string customerName;
    public ServiceType requestedService;
    public Pet pet;
    private TaskManager taskManager;
    private NavMeshAgent navMeshAgent;

    public Transform destination;
    private Queue<Action> actionQueue;

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
        taskManager.CreateTask(this, requestedService);
        SetTarget(TaskManager.Instance.door);
        actionQueue.Enqueue(() => WaitPetFinish());
    }

    void WaitPetFinish()
    {

    }

    void SetTarget(Transform target)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.updateRotation = true;
        destination = target;
        navMeshAgent.SetDestination(destination.position);
    }
    public void ReachDestination()
    {
        if (actionQueue.Count > 0)
        {
            navMeshAgent.isStopped = true;
            navMeshAgent.updateRotation = false;
            navMeshAgent.velocity = Vector3.zero;
            //navMeshAgent.SetDestination(transform.position);
            actionQueue.Dequeue().Invoke();
        }
    }
    public void Leave()
    {
        // Customer leaves after service is done
        Debug.Log($"{customerName} is leaving the spa.");
        Destroy(gameObject);  // For now, we'll just destroy the customer object.
    }
}