using Cysharp.Threading.Tasks;
using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cashier : Entity
{
    IdleTask idleTask;
    Counter assignedCounter;
    private void Start()
    {
        assignedCounter = GetComponentInParent<Counter>();
    }
    public async UniTask TakeTask(IdleTask _idleTask)
    {
        if (idleTask != null) 
            idleTask = _idleTask;
        var petZone = TaskManager.Instance.petZones[_idleTask.pet.petType];
        await SetTarget(TaskManager.Instance.petZones[_idleTask.pet.petType].CashierDropoffTF);
        await petZone.ReceiveTask(_idleTask);
        await SetTarget(assignedCounter.CashierStand(), assignedCounter.CashierStand().rotation);
    }    
    void OrderTask()
    {

    }    
}
