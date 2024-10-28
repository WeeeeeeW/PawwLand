using Cysharp.Threading.Tasks;
using System.Collections;
using System.Threading;

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
        var petZone = TaskManager.Instance.petZones[_idleTask.pet.PetType];
        await SetTarget(TaskManager.Instance.petZones[_idleTask.pet.PetType].CashierDropoffTF);
        await petZone.ReceiveTask(_idleTask);
        await SetTarget(assignedCounter.CashierStand(), assignedCounter.CashierStand().rotation);
    }

    public override void Patrol()
    {
        throw new System.NotImplementedException();
    }
}
