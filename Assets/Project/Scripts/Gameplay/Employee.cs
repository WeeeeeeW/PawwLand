using Pathfinding;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Employee : Entity
{
    public string employeeName;
    [ReadOnly] public bool isAvailable = true;
    [SerializeField][ReadOnly] private Stack<IdleTask> currentTasks;
    [SerializeField][ReadOnly] private IdleTask currentTask;
    private TaskManager taskManager;
    [SerializeField] float efficiency = 1f;
    bool patroling = false;
    void Awake()
    {
        navMeshAgent = GetComponent<FollowerEntity>();
    }
    private void Start()
    {
        taskManager = TaskManager.Instance;
        currentTasks = new Stack<IdleTask>();
        //if (isAvailable)
        //{
        //    Patrol();
        //}
    }

    private void Update()
    {
        if (isAvailable && (navMeshAgent.reachedEndOfPath || !navMeshAgent.hasPath) && !patroling)
        {
            StartCoroutine("Patrol");
        }
    }

    public override IEnumerator Patrol()
    {
        patroling = true;
        navMeshAgent.updateRotation = true;
        if (!isAvailable)
        {
            yield break;
        }

        Vector3 destination1 = GameManager.Instance.RandomPatrolPosition();
        navMeshAgent.SetDestination(destination1);
        yield return new WaitUntil(() => navMeshAgent.reachedEndOfPath || !navMeshAgent.hasPath);
        yield return new WaitForSeconds(3f);
        patroling = false;
    }

    public void AssignTask(IdleTask task)
    {
        Debug.Log($"{employeeName} is assigned with {task.serviceType}");
        isAvailable = false;
        patroling = false;
        currentTasks.Push(task);
        switch (task.serviceType)
        {
            case ServiceType.Grooming:
                currentTasks.Push(new IdleTask(task.customer, task.pet, ServiceType.Bath));
                break;
        }
        StopCoroutine("Patrol");
        //if (Vector3.Distance(transform.position, TaskManager.Instance.petzone.employeeDoor.position) < 1f)
        //    ReachDestination();
    }

    private IEnumerator PerformTask(TaskStation _taskStation)
    {
        if (currentTasks != null)
        {
            Vector3 startRotation = transform.forward;
            float rotationSpeed = 360f;
            Vector3 desiredRotation = _taskStation.transform.position - transform.position;
            desiredRotation.y = 0;
            float rotationAngle = Vector3.Angle(transform.forward, desiredRotation);
            float rotationDuration = rotationAngle / rotationSpeed;
            float elapsedTime = 0f;

            while (elapsedTime < rotationDuration)
            {
                elapsedTime += Time.deltaTime;
                transform.forward = Vector3.Lerp(startRotation, desiredRotation, elapsedTime / rotationDuration);
                yield return null;
            }


            Debug.Log($"{employeeName} starts performing {currentTask.serviceType} for {currentTask.customer.customerName}");
            //Place Pet on Task Pos
            currentTask.pet.AssignToStation(_taskStation);
            yield return new WaitForSeconds(_taskStation.taskDuration);
            UnsubscribeToStation(_taskStation);
            if (currentTasks.Count > 0)
            {
                yield break;
            }
            //Pickup Pet
            currentTask.pet.transform.parent = petHolder;
            currentTask.pet.transform.localPosition = Vector3.zero;

        }
    }

    IEnumerator ReturnPet()
    {
        yield return new WaitForSeconds(.2f);
        currentTask.pet.transform.parent = taskManager.petzone.transform;
        currentTask.pet.transform.localPosition = Vector3.zero;
        OnTaskComplete();
    }

    public void OnTaskComplete()
    {
        isAvailable = true;
        currentTask.customer?.ProceedPayment();
        Debug.Log($"{employeeName} completed the task and is now available.");
        currentTask = null;
        taskManager.AssignTaskFromQueue();
    }

    Action _actionRef;
    public void SubscribeToStation(TaskStation _station)
    {
        _station.advanceQueue += _actionRef;
    }
    public void UnsubscribeToStation(TaskStation _station)
    {
        _station.advanceQueue -= _actionRef;
    }
}
