using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TaskStation : MonoBehaviour
{
    public Transform EntityTarget;
    private QueueManager queueManager;
    [SerializeField] List<Transform> queuePos;
    [SerializeField] float taskDuration;
    private bool isBusy = false;
    private void Start()
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
        await _employee.SetTarget(EntityTarget);

        // Once arrived, request service
        await UniTask.WaitForSeconds(taskDuration);
        _employee.PickupPet(_employee.currentTask.pet);
        _employee.NextTask();



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

}
