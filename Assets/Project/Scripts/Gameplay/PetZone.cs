using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Pathfinding.Drawing.DrawingData;

public class PetZone : MonoBehaviour
{
    public Transform CashierDropoffTF, EmployeePickupPoint;
    float taskDuration = .2f;
    private QueueManager queueManager;
    [SerializeField] List<Transform> queuePos;
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
            _employee.PickupPet(_employee.currentTask.pet);
            _employee.GoToExecuteTask();
        }
        else
        {
            var _pet = _employee.DropOffPet();
            _pet.transform.parent = transform;
            _pet.transform.localPosition = Vector3.zero;
            _employee.FinishTask();
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

    public async UniTask ReceiveTask(IdleTask _idleTask)
    {
        await UniTask.WaitForSeconds(taskDuration);
        _idleTask.pet.transform.parent = transform;
        _idleTask.pet.transform.localPosition = Vector3.zero;
        TaskManager.Instance.CreateTask(_idleTask);
    }
}
