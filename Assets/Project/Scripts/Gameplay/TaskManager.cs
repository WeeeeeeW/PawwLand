using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : Singleton<TaskManager>
{
    public Transform employeeCounter, door;
    [SerializeField] private GameObject[] customers;

    public Queue<IdleTask> taskQueue = new Queue<IdleTask>();
    private new void Awake()
    {
        base.Awake();
        //SpawnCustomer();
    }

    [Button]
    void SpawnCustomer()
    {
        //StartCoroutine(SpawnCustomerCoroutine());
        Instantiate(customers[Random.Range(0, customers.Length)], door.position, Quaternion.identity);
    }

    IEnumerator SpawnCustomerCoroutine()
    {
        while (true)
        {
            Instantiate(customers[Random.Range(0, customers.Length)], door.position, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(7f, 9f));
        }
    }
}
