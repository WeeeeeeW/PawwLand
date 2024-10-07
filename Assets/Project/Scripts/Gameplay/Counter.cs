using DG.Tweening.Core.Easing;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Counter : BaseTaskStation
{
    [SerializeField]public Manager manager;
    public Transform customerOut, customerIn, queueStart;
    Queue<Customer> customerQueue;
    public Action callNextCustomer;
    // Start is called before the first frame update
    void Start()
    {
        manager.AssignToCounter(this);
        customerQueue = new Queue<Customer>();
    }

    public override IEnumerator AdvanceQueue()
    {
        yield return new WaitUntil(() => manager.isAvailable);
        yield return new WaitForSeconds(taskDuration / manager.efficiency);
        Customer _customer = customerQueue.Dequeue();
        Debug.Log($"{_customer.customerName} requests {_customer.requestedService}");
        manager.AssignTask(_customer.requestedService, _customer.pet);
        callNextCustomer?.Invoke();
        _customer.UnsubscribeToCounter(this);
    }

    public override void QueueUp(Entity _entity)
    {
        Customer _customer = _entity.GetComponent<Customer>();
        customerQueue.Enqueue(_customer);
        for (int i = customerQueue.Count; i > 1; i--)
        {
            var x = i - 1;
            _customer.AddActionQueue(() => _customer.SetQueueTarget(customerIn.transform.position + customerIn.forward * x * 1.5f));
        }
        _customer.AddActionQueue(() => _customer.SetQueueTarget(customerIn));
        _customer.InvokeQueue();
    }
}
