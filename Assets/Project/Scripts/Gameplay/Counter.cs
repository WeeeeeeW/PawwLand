using DG.Tweening.Core.Easing;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Counter : BaseTaskStation
{
    [SerializeField] public Manager manager;
    public Transform customerOut, customerIn, queueStart;
    public Action callNextCustomer;
    // Start is called before the first frame update
    void Start()
    {
        manager.AssignToCounter(this);
        entityQueue = new Queue<Entity>();
    }

    public override IEnumerator AdvanceQueue(bool requestService)
    {
        yield return new WaitUntil(() => manager.isAvailable);
        yield return new WaitForSeconds(taskDuration / manager.efficiency);
        Customer _customer = (Customer)entityQueue.Dequeue();
        if (requestService)
        {
            Debug.Log($"{_customer.customerName} requests {_customer.requestedService}");
            manager.AssignTask(_customer.requestedService, _customer.pet);
        }
        callNextCustomer?.Invoke();
        _customer.UnsubscribeToCounter(this);
    }

    public override void QueueUp(Entity _entity)
    {
        Customer _customer = _entity.GetComponent<Customer>();
        entityQueue.Enqueue(_customer);
        for (int i = entityQueue.Count; i > 1; i--)
        {
            var x = i - 1;
            _customer.AddActionQueue(() => _customer.SetQueueTarget(customerIn.transform.position + customerIn.forward * x * 1.5f));
        }
        //queueStart.transform.position = customerIn.transform.position + customerIn.forward * entityQueue.Count * 1.5f;
        _customer.AddActionQueue(() => _customer.SetQueueTarget(customerIn));
        _customer.InvokeQueue();
    }
}
