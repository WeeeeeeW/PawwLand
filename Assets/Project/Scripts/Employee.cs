using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using Cysharp.Threading.Tasks;

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

    public async void AssignTask(IdleTask task)
    {
        isAvailable = false;
        navMeshAgent.SetDestination(TaskManager.Instance.employeeCounter.position);
        currentTask = task;
        await UniTask.WaitUntil(() => navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && (navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f));
        // Move to task location (simulate with a destination)
        MoveToTaskStation();
    }

    private async void MoveToTaskStation()
    {
        TaskStation taskStation = currentTask.GetTaskLocation();
        Vector3 taskStationPosition = taskStation.doTaskPosition.position;
        navMeshAgent.SetDestination(taskStationPosition);
        await UniTask.WaitUntil(() => navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && (navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f));
        
        Vector3 startRotation = transform.forward;
        float rotationSpeed = 90f;
        Vector3 desiredRotation = taskStation.transform.position - transform.position;
        desiredRotation.y = 0;
        float rotationAngle = Vector3.Angle(transform.forward, desiredRotation);
        float rotationDuration = rotationAngle / rotationSpeed;
        float elapsedTime = 0f;
        while (Vector3.Angle(transform.forward, desiredRotation) > 2)
        {
            elapsedTime += Time.deltaTime;
            transform.forward = Vector3.Lerp(startRotation, desiredRotation, elapsedTime);
            await UniTask.DelayFrame(1);
        }
        PerformTask();

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
