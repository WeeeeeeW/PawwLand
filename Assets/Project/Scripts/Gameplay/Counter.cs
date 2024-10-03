using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Counter : MonoBehaviour
{
    [SerializeField] Manager manager;
    public Transform customerOut, customerIn, queueStart;
    Queue<Customer> customerQueue;
    public Action callNextCustomer;
    // Start is called before the first frame update
    void Start()
    {
        manager.AssignToCounter(this);
        customerQueue = new Queue<Customer>();
    }
    public void QueueUpCustomer(Customer _customer)
    {
        //_customer.SetQueueTarget(customerIn.transform.position + customerIn.forward * customerQueue.Count);
        customerQueue.Enqueue(_customer);
        for (int i = customerQueue.Count; i > 1; i--)
        {
            var x = i - 1;
            Debug.Log(customerIn.transform.position);
            _customer.AddActionQueue(() => _customer.SetQueueTarget(customerIn.transform.position + customerIn.forward * x * 1.5f));
        }
        _customer.AddActionQueue(() => _customer.SetQueueTarget(customerIn));
        _customer.InvokeQueue();
    }
    public IEnumerator ServeCustomer(Customer _customer)
    {
        yield return new WaitUntil(() => manager.isAvailable);
        yield return new WaitForSeconds(.2f / manager.efficiency);
        Debug.Log($"{_customer.customerName} requests {_customer.requestedService}");
        manager.AssignTask(_customer.requestedService, _customer.pet);
        customerQueue.Dequeue();
        callNextCustomer?.Invoke();
    }
}
