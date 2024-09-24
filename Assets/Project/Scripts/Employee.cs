using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

public class Employee : MonoBehaviour
{
    public string employeeName;
    public bool isAvailable = true;
    private IdleTask currentTask;
    private TaskManager taskManager;
    private NavMeshAgent navMeshAgent;

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        taskManager = TaskManager.Instance;
    }

    public void AssignTask(IdleTask task)
    {
        isAvailable = false;
        currentTask = task;
        Debug.Log($"{employeeName} assigned to {task.serviceType} for {task.customer.customerName}");

        // Move to task location (simulate with a destination)
        MoveToTaskStation();
    }

    private void MoveToTaskStation()
    {
        // For simplicity, use a random position for task station (replace this with actual task station position)
        Vector3 taskStationPosition = currentTask.GetTaskLocation().transform.position;
        navMeshAgent.SetDestination(taskStationPosition);
    }

    void Update()
    {
        if (!isAvailable && !navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            PerformTask();
        }
    }

    private void PerformTask()
    {
        if (currentTask != null)
        {
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
