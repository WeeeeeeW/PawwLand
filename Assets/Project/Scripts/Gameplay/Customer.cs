using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

public class Customer : MonoBehaviour
{
    public string customerName;
    public ServiceType requestedService;
    public Pet pet;
    private TaskManager taskManager;
    private NavMeshAgent navMeshAgent;
    bool assignedTask = false;

    void Start()
    {
        // Assign the task manager
        taskManager = TaskManager.Instance;
        navMeshAgent = GetComponent<NavMeshAgent>();

        MoveToCounter();
    }

    [Button]
    public void MoveToCounter()
    {
        // Request a task based on service type
        //Debug.Log($"{customerName} is coming in with {pet.petName}.");
        navMeshAgent.SetDestination(TaskManager.Instance.customerCounter.position);
        assignedTask = false;
    }

    void Update()
    {
        // Check if customer has reached the counter
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            RegisterTask();
        }
    }

    void RegisterTask()
    {
        if (assignedTask)
            return;
        Debug.Log($"{customerName} requests {requestedService}");
        taskManager.CreateTask(this, requestedService);
        assignedTask = true;
    }

    public void Leave()
    {
        // Customer leaves after service is done
        Debug.Log($"{customerName} is leaving the spa.");
        Destroy(gameObject);  // For now, we'll just destroy the customer object.
    }
}