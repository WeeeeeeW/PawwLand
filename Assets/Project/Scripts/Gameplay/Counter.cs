using Cysharp.Threading.Tasks;
using DG.Tweening.Core.Easing;
using Pathfinding;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Counter : BaseTaskStation
{
    [SerializeField] public Manager manager;
    public Transform customerOut, customerIn;
    // Start is called before the first frame update
    void Start()
    {
        manager.AssignToCounter(this);
        entityQueue = new List<Entity>();
    }
    private void Update()
    {
        if(isAvailable)
            TakeOrder();
    }
    public async void TakeOrder()
    {
        if (entityQueue.Count <= 0)
        {
            return;
        }
        isAvailable = false;
        Customer _customer = (Customer)entityQueue[0];
        await _customer.SetTarget(entityTaskPosition);
        _customer.RequestService();
    }

    //public override IEnumerator AdvanceQueue(bool requestService)
    //{
    //    yield return new WaitUntil(() => manager.isAvailable);
    //    yield return new WaitForSeconds(taskDuration / manager.efficiency);
    //    //_customer.GetComponent<FollowerEntity>().enableLocalAvoidance = true;
    //    //if (requestService)
    //    //{
    //    //    //Debug.Log($"{_customer.customerName} requests {_customer.requestedService}");
    //    //    manager.AssignTask(_customer.requestedService, _customer.pet);
    //    //}
    //    //advanceQueue?.Invoke();
    //    //_customer.UnsubscribeToCounter(this);
    //}

}
