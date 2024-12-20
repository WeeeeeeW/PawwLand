using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : Singleton<TaskManager>
{
    public Transform employeeCounter, door;
    public Counter[] counters;
    [SerializeField] Employee[] employees;
    public Dictionary<PetType, PetZone> petZones;
    public Dictionary<ServiceType, List<TaskStation>> taskStations;
    [SerializeField] private GameObject[] customers;

    public Queue<IdleTask> taskQueue = new Queue<IdleTask>();
    private new void Awake()
    {
        base.Awake();
        //SpawnCustomer();
    }
    public void CreateTask(IdleTask _idleTask)
    {
        AssignTask(_idleTask);
    }

    public void AssignTask(IdleTask task)
    {
        Employee availableEmployee = GetAvailableEmployee();
        if (availableEmployee != null)
        {
            availableEmployee.TakeTask(task);
        }
        else
        {
            taskQueue.Enqueue(task);
            Debug.Log("No available employees. Task added to queue.");
        }
    }
    public bool AssignTaskFromQueue()
    {
        if (taskQueue.Count > 0)
        {
            IdleTask nextTask = taskQueue.Dequeue();
            AssignTask(nextTask);
            return true;
        }
        return false;
    }

    public TaskStation GetStationForService(ServiceType serviceType)
    {
        return taskStations[serviceType][0];  // Get the first available station for the service
    }

    private Employee GetAvailableEmployee()
    {
        // Logic to find available employee
        foreach(Employee employee in employees)
        {
            if(employee.IsAvailable)
                return employee;
        }
        return null;
    }
    [Button]
    void SpawnCustomer()
    {
        //StartCoroutine(SpawnCustomerCoroutine());
        Instantiate(customers[Random.Range(0, customers.Length)], door.position, door.rotation);
    }

    IEnumerator SpawnCustomerCoroutine()
    {
        while (true)
        {
            Instantiate(customers[Random.Range(0, customers.Length)], door.position, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(7f, 9f));
        }
    }
}
