using Sirenix.OdinInspector;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public string customerName;
    public ServiceType requestedService;
    private TaskManager taskManager;

    void Start()
    {
        // Assign the task manager
        taskManager = TaskManager.Instance;
        RequestService();
    }

    [Button]
    public void RequestService()
    {
        // Request a task based on service type
        Debug.Log($"{customerName} requests {requestedService}");
        taskManager.CreateTask(this, requestedService);
    }

    public void Leave()
    {
        // Customer leaves after service is done
        Debug.Log($"{customerName} is leaving the spa.");
        Destroy(gameObject);  // For now, we'll just destroy the customer object.
    }
}