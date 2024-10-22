using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;

public class Counter : MonoBehaviour
{
    private QueueManager queueManager;
    [SerializeField] Cashier cashier;
    [SerializeField] Transform customerServeTF, counterExitTF, cashierTF;
    [SerializeField] List<Transform> queuePos;
    [SerializeField, ReadOnly] private bool isBusy = false;
    [SerializeField] float orderDuration;
    void Awake()
    {
        queueManager = new QueueManager(queuePos);
    }

    public void AssignCustomerToCounter(Customer _customer)
    {
        queueManager.AddToQueue(_customer);
        if (!isBusy || queueManager.GetQueueCount() <= 1)
        {
            StartTask(_customer);
        }
    }
    private async void StartTask(Customer _customer)
    {
        await UniTask.WaitUntil(() => !isBusy);
        isBusy = true;
        // Move customer to counter
        await _customer.SetTarget(customerServeTF);

        // Once arrived, request service
        await UniTask.WaitForSeconds(orderDuration);




        // Process next in queue
        queueManager.RemoveFromQueue();
        ProcessNextInQueue();
        if (!_customer.Paying)
        {
            _customer.RequestServiceAtCounter();
            cashier.PickupPet(_customer.pet);
            await TakeOrder(new IdleTask(_customer, _customer.requestedService, _customer.pet));
        }
        else
        {
            _customer.PickupPet();
            Debug.Log($"{_customer} paid <color=green>$$$</color>");
        }
        isBusy = false;
        // Mark the station as free after task completion

    }

    public void ProcessNextInQueue()
    {
        if (queueManager.HasWaitingEntities())
        {
            Customer nextCustomer = (Customer)queueManager.GetNextInQueue();
            StartTask(nextCustomer);
        }
    }

    public async UniTask TakeOrder(IdleTask _idleTask)
    {
        await cashier.TakeTask(_idleTask);
    }
    public Transform CashierStand()
    {
        return cashierTF;
    }
    public Transform CounterExit()
    {
        return counterExitTF;
    }
}
