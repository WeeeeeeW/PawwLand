using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var employee = other.GetComponent<Employee>();
        if(employee != null)
        {
            if(employee.destination == transform)
                employee.ReachDestination();
        }

        var customer = other.GetComponent<Customer>();
        if (customer != null)
        {
            if (customer.destination == transform)
                customer.ReachDestination();
        }
    }
}
