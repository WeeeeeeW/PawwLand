using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Pathfinding.Drawing.DrawingData;

public class PetZone : MonoBehaviour
{
    public Transform CashierDropoffTF, EmployeePickupPoint, CustomerPickupPoint;
    float taskDuration = .2f;
    private QueueManager queueManager;
    [SerializeField] List<Transform> queuePos;
    [SerializeField] List<Transform> patrolArea;
    private bool isBusy = false;

    void Awake()
    {
        queueManager = new QueueManager(queuePos);
    }

    public void AssignEmployeeToQueue(Employee _employee)
    {
        queueManager.AddToQueue(_employee);
        if (!isBusy)
        {
            StartTask(_employee);
        }
    }

    private async void StartTask(Employee _employee)
    {
        await UniTask.WaitUntil(() => !isBusy);
        isBusy = true;
        // Move employee to station
        await _employee.SetTarget(EmployeePickupPoint);

        // Once arrived, request service
        await UniTask.WaitForSeconds(taskDuration);
        if (!_employee.IsComplete)
        {
            RemovePetFromZone(_employee.currentTask.Pet);
            _employee.PickupPet(_employee.currentTask.Pet);
            _employee.GoToExecuteTask();
        }
        else
        {
            var _pet = _employee.DropOffPet();

            _employee.FinishTask();
            AssignPetToZone(_pet);
            _pet.Owner.MakePayement();
        }



        // Process next in queue
        queueManager.RemoveFromQueue();
        ProcessNextInQueue();
        isBusy = false;
        // Mark the station as free after task completion

    }

    public void ProcessNextInQueue()
    {
        if (queueManager.HasWaitingEntities())
        {
            Employee nextEmployee = (Employee)queueManager.GetNextInQueue();
            StartTask(nextEmployee);
        }
    }
    public Vector3 RandomPatrolPosition()
    {
        return new Vector3(Random.Range(patrolArea[0].position.x, patrolArea[1].position.x), 0, Random.Range(patrolArea[0].position.z, patrolArea[1].position.z));
    }
    public async UniTask ReceiveTask(IdleTask _idleTask)
    {
        await UniTask.WaitForSeconds(taskDuration);
        AssignPetToZone(_idleTask.Pet);
        TaskManager.Instance.CreateTask(_idleTask);
    }

    void AssignPetToZone(Pet _pet)
    {
        _pet.transform.parent = transform;
        _pet.transform.localPosition = Vector3.zero;
        _pet.CurrentZone = this;
        _pet.Patrol();
    }
    void RemovePetFromZone(Pet _pet)
    {
        _pet.transform.parent = transform;
        _pet.transform.localPosition = Vector3.zero;
        _pet.CurrentZone = null;
        _pet.StopPatrolling();
    }
}
