using UnityEngine;

public class Pet : MonoBehaviour
{
    public string petName;
    public Customer owner; // Reference to the customer that owns the pet
    public PetType petType; // Enum to define the type of pet (Dog, Cat, etc.)

    public void AssignToOwner(Customer customer)
    {
        owner = customer;
        Debug.Log($"{petName} has been assigned to {owner.customerName}.");
    }

    // Add any additional methods for pet behavior or animations
}
