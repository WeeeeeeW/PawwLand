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
    [HideInInspector] public bool isAvailable = true;
    [SerializeField][ReadOnly] private IdleTask currentTask;
    private TaskManager taskManager;
    [SerializeField] Transform petHolder;
    [SerializeField] float efficiency = 1f;
    bool patroling = false;
    void Awake()
    {
        navMeshAgent = GetComponent<FollowerEntity>();
    }
    private void Start()
    {
        taskManager = TaskManager.Instance;
        actionQueue = new Queue<Action>();
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
        GraphNode randomNode;

        // For grid graphs
        var grid = AstarPath.active.data.gridGraph;
        randomNode = grid.nodes[Random.Range(0, grid.nodes.Length)];

        // Use the center of the node as the destination for example
        var destination1 = (Vector3)randomNode.position;
        navMeshAgent.SetDestination(destination1);
        yield return new WaitUntil(() => navMeshAgent.reachedEndOfPath || !navMeshAgent.hasPath);
        yield return new WaitForSeconds(3f);
        patroling = false;
    }

    public void AssignTask(IdleTask task)
    {
        isAvailable = false;
        patroling = false;
        currentTask = task;
        StopCoroutine("Patrol");
        StartCoroutine(PickupPet());
        //actionQueue.Enqueue(() => StartCoroutine(PickupPet(task.pet)));
        if (Vector3.Distance(transform.position, TaskManager.Instance.petzone.employeeDoor.position) < 1f)
            ReachDestination();
    }

    public IEnumerator PickupPet()
    {
        SetTarget(currentTask.pet.station.employeeTaskPosition);
        yield return new WaitUntil(() => navMeshAgent.reachedDestination);
        yield return new WaitForSeconds(.2f);
        currentTask.pet.AssignToStation(null);
        currentTask.pet.transform.parent = petHolder;
        currentTask.pet.transform.localPosition = Vector3.zero;
        MoveToTaskStation();
    }

    private void MoveToTaskStation()
    {
        TaskStation taskStation = currentTask.GetTaskLocation();
        SetTarget(taskStation.employeeTaskPosition);
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
            //Place Pet on Task Pos
            currentTask.pet.AssignToStation(_taskStation);
            yield return new WaitForSeconds(_taskStation.taskDuration);
            //Pickup Pet
            currentTask.pet.transform.parent = petHolder;
            currentTask.pet.transform.localPosition = Vector3.zero;
            SetTarget(taskManager.petzone.employeeDoor);
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
