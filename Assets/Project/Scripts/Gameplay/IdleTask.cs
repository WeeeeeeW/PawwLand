using UnityEngine;

public class IdleTask
{
    public Customer customer;
    public ServiceType serviceType;
    public bool isCompleted = false;

    public IdleTask(Customer customer, ServiceType serviceType)
    {
        this.customer = customer;
        this.serviceType = serviceType;
    }

    public void PerformTask(Employee employee)
    {
        Debug.Log($"{employee.name} is performing {serviceType} for {customer.customerName}");
        // Simulate task completion
        CompleteTask(employee);
    }

    private void CompleteTask(Employee employee)
    {
        isCompleted = true;
        Debug.Log($"{employee.name} completed {serviceType} for {customer.customerName}");
        customer.Leave();  // Customer leaves once the task is done.
        employee.OnTaskComplete();
    }

    public TaskStation GetTaskLocation()
    {
        return TaskManager.Instance.stations[serviceType][0];
    }
}
