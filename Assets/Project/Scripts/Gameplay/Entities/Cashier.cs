using Cysharp.Threading.Tasks;
using System.Collections;
using System.Threading;
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
        var petZone = TaskManager.Instance.petZones[_idleTask.Pet.PetType];
        await SetTarget(TaskManager.Instance.petZones[_idleTask.Pet.PetType].CashierDropoffTF);
        DropOffPet();
        await petZone.ReceiveTask(_idleTask);
        await SetTarget(assignedCounter.CashierStand(), assignedCounter.CashierStand().rotation);
    }

    public override void Patrol(Transform[] patrolArea)
    {
        throw new System.NotImplementedException();
    }

    public override void OnEnterQueue()
    {
        throw new System.NotImplementedException();
    }
}
