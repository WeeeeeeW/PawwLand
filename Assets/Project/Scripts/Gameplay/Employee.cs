using Pathfinding;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Employee : Entity
{
    public string employeeName;
    [HideInInspector] public bool isAvailable = true;
    [SerializeField][ReadOnly] private IdleTask currentTask;
    private TaskManager taskManager;
    [SerializeField] Transform petHolder;
    [SerializeField] float efficiency = 1f;
    void Awake()
    {
        navMeshAgent = GetComponent<FollowerEntity>();
    }
    private void Start()
    {
        taskManager = TaskManager.Instance;
        actionQueue = new Queue<Action>();
    }

    public void AssignTask(IdleTask task)
    {
        isAvailable = false;
        SetTarget(TaskManager.Instance.petzone.door);

        currentTask = task;
        actionQueue.Enqueue(() => StartCoroutine(PickupPet(task.pet)));
        if(Vector3.Distance(transform.position, TaskManager.Instance.petzone.door.position) < 1f)
            ReachDestination();
    }

    public IEnumerator PickupPet(Pet _pet)
    {
        yield return new WaitForSeconds(.2f);
        _pet.transform.parent = petHolder;
        _pet.transform.localPosition = Vector3.zero;
        MoveToTaskStation();
    }

    private void MoveToTaskStation()
    {
        BaseTaskStation taskStation = currentTask.GetTaskLocation();
        SetTarget(taskStation.employeeTaskPosition);
        actionQueue.Enqueue(() => StartCoroutine(PerformTask(taskStation)));

        //PerformTask();

    }

    private IEnumerator PerformTask(BaseTaskStation _taskStation)
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
            //Place Pet on Task Pos
            currentTask.pet.transform.parent = _taskStation.taskPosition;
            currentTask.pet.transform.localPosition = Vector3.zero;
            yield return new WaitForSeconds(_taskStation.taskDuration);
            //Pickup Pet
            currentTask.pet.transform.parent = petHolder;
            currentTask.pet.transform.localPosition = Vector3.zero;
            SetTarget(taskManager.petzone.door);
            actionQueue.Enqueue(() => StartCoroutine(ReturnPet()));
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
}
