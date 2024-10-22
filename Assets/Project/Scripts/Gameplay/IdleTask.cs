using UnityEngine;

public class IdleTask
{
    public Customer customer;
    public ServiceType serviceType;
    public Pet pet;

    public IdleTask(Customer _customer, ServiceType _serviceType, Pet _pet)
    {
        customer = _customer;
        serviceType = _serviceType;
        pet = _pet;
    }
    public void ExecuteTask(Employee employee)
    {
        // Employee has already arrived at the destination
        //employee.PerformTask(this);
    }

    public void CompleteTask()
    {
        Debug.Log($"Task for {customer.customerName} ({serviceType}) completed.");
        customer.Leave();
    }
}
