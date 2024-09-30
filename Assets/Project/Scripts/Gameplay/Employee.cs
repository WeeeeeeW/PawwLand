using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Employee : Entity
{
    public string employeeName;
    public bool isAvailable = true;
    private IdleTask currentTask;
    private TaskManager taskManager;

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        taskManager = TaskManager.Instance;
        actionQueue = new Queue<Action>();
    }

    public void AssignTask(IdleTask task)
    {
        isAvailable = false;
        SetTarget(TaskManager.Instance.cageDoor);
        currentTask = task;
        // Move to task location (simulate with a destination)
        actionQueue.Enqueue(() => MoveToTaskStation());
    }

    private void MoveToTaskStation()
    {
        TaskStation taskStation = currentTask.GetTaskLocation();
        SetTarget(taskStation.doTaskPosition);
        actionQueue.Enqueue(() => StartCoroutine(PerformTask(taskStation)));

        //PerformTask();

    }

    private IEnumerator PerformTask(TaskStation _taskStation)
    {
        if (currentTask != null)
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
            currentTask.PerformTask(this);
        }
    }

    public void OnTaskComplete()
    {
        isAvailable = true;
        Debug.Log($"{employeeName} completed the task and is now available.");
        taskManager.AssignTaskFromQueue();
    }
}
