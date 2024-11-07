using UnityEngine;

public class IdleTask
{
    public Customer Customer;
    public ServiceType ServiceType;
    public Pet Pet;
    public Transform Target;
    public GameObject TaskObject;

    public IdleTask(Customer _customer, ServiceType _serviceType, Pet _pet)
    {
        Customer = _customer;
        ServiceType = _serviceType;
        Pet = _pet;
    }
    public IdleTask(ServiceType _serviceType, Transform _target, GameObject _taskObject)
    {
        ServiceType = _serviceType;
        Target = _target;
        TaskObject = _taskObject;
    }
}
