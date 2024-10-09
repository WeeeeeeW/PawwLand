using UnityEngine;
using UnityEngine.InputSystem;

public class Pet : MonoBehaviour
{
    public string petName;
    public Customer owner; // Reference to the customer that owns the pet
    public PetType petType; // Enum to define the type of pet (Dog, Cat, etc.)
    public TaskStation station;
    public void AssignToOwner(Customer customer)
    {
        owner = customer;
        Debug.Log($"{petName} has been assigned to {owner.customerName}.");
    }
    public void AssignToStation(TaskStation _station)
    {
        if (_station != null)
        {
            transform.parent = _station.transform;
            if(_station.taskPosition == null)
                transform.localPosition = Vector3.zero;
            else
                transform.localPosition = _station.taskPosition.localPosition;
        }
        else
        {
            transform.parent = null;
        }
        station = _station;
    }

    // Add any additional methods for pet behavior or animations
}
