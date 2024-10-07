using System;
using UnityEngine;

[Serializable]
public class IdleTask
{
    public Customer customer;
    public Pet pet;
    public ServiceType serviceType;
    public bool isCompleted = false;

    public IdleTask(Customer customer, Pet pet, ServiceType serviceType)
    {
        this.customer = customer;
        this.pet = pet;
        this.serviceType = serviceType;
    }


    private void CompleteTask(Employee employee)
    {
        isCompleted = true;
        Debug.Log($"{employee.name} completed {serviceType} for {customer.customerName}");
        customer.Leave();  // Customer leaves once the task is done.
        employee.OnTaskComplete();
    }

    public BaseTaskStation GetTaskLocation()
    {
        return TaskManager.Instance.stations[serviceType][0];
    }
}
