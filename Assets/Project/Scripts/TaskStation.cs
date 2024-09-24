using UnityEngine;

public class TaskStation : MonoBehaviour
{
    public string stationName;
    public ServiceType serviceType;
    public bool stationAvailability;
    public void AssignCustomerToStation(Customer customer)
    {
        Debug.Log($"{customer.customerName} assigned to {stationName}.");
        // Handle any logic for station-specific behavior here.
    }
}
