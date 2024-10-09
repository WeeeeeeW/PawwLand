using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : Singleton<TaskManager>
{
    public Transform employeeCounter, door;
    public Counter counter;
    [SerializeField] private GameObject[] customers;

    public Queue<IdleTask> taskQueue = new Queue<IdleTask>();
    public Manager manager;
    public List<Employee> employees;
    public Dictionary<ServiceType, List<TaskStation>> stations;
    public Petzone petzone;
    private new void Awake()
    {
        base.Awake();
        var allStations = FindObjectsOfType<TaskStation>();
        foreach (var station in allStations)
        {
            if(stations.ContainsKey(station.serviceType))
                stations[station.serviceType].Add(station);
        }
    }
    public void CreateTask(Customer customer, Pet pet, ServiceType serviceType)
    {
        IdleTask newTask = new IdleTask(customer, pet, serviceType);
        Debug.Log($"Task created for {customer.customerName}: {pet.petName} {serviceType}");
        AssignTask(newTask);
    }
    public void AssignPetToZone(Pet pet)
    {
        pet.AssignToStation(petzone);
    }

    public void AssignTask(IdleTask task)
    {
        Employee availableEmployee = GetAvailableEmployee();
        if (availableEmployee != null)
        {
            availableEmployee.AssignTask(task);
        }
        else
        {
            Debug.Log("No available employees. Adding task to the queue.");
            taskQueue.Enqueue(task);
        }
    }

    public void AssignTaskFromQueue()
    {
        if (taskQueue.Count > 0)
        {
            IdleTask nextTask = taskQueue.Dequeue();
            AssignTask(nextTask);
        }
    }
    private Employee GetAvailableEmployee()
    {
        foreach (Employee employee in employees)
        {
            if (employee.isAvailable)
            {
                return employee;
            }
        }
        return null;
    }

    [Button]
    void SpawnCustomer()
    {
        Instantiate(customers[Random.Range(0, customers.Length)], door.position, Quaternion.identity);
    }
}
