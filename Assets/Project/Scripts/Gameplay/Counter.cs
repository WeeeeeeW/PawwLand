using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : MonoBehaviour
{
    private QueueManager queueManager;
    [SerializeField] List<Transform> queuePos;
    // Start is called before the first frame update
    void Start()
    {
        queueManager = new QueueManager(queuePos);
    }

    public void AssignCustomerToCounter(Customer _customer)
    {

    }
}
