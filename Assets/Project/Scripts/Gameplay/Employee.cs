using Cysharp.Threading.Tasks;
using DG.Tweening.Core.Easing;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Employee : Entity
{
    public IdleTask currentTask; 
    private Queue<IdleTask> tasks;
    public bool IsAvailable => currentTask == null;
    public bool IsComplete = false;
    CancellationTokenSource cancellationTokenSource;
    protected override void Start()
    {
        base.Start();
        cancellationTokenSource = new CancellationTokenSource();
        Patrol(cancellationTokenSource.Token);
    }
    public void TakeTask(IdleTask _task)
    {
        cancellationTokenSource.Cancel();
        IsComplete = false;
        tasks = new Queue<IdleTask>();
        switch(_task.serviceType)
        {
            case ServiceType.Bath:
                tasks.Enqueue(_task);
                break;
            case ServiceType.Grooming:
                tasks.Enqueue(new IdleTask(_task.customer, ServiceType.Bath, _task.pet));
                tasks.Enqueue(_task);
                break;

        }    
        currentTask = tasks.Dequeue();
        TaskManager.Instance.petZones[currentTask.pet.petType].AssignEmployeeToQueue(this);
    }

    public void GoToExecuteTask()
    {
        Debug.Log(currentTask.serviceType);
        Debug.Log(TaskManager.Instance.taskStations[currentTask.serviceType][0].name);
        TaskManager.Instance.taskStations[currentTask.serviceType][0].AssignEmployeeToQueue(this);
    }

    public void NextTask()
    {
        if (tasks.Count > 0)
        {
            currentTask = tasks.Dequeue();
            TaskManager.Instance.taskStations[currentTask.serviceType][0].AssignEmployeeToQueue(this);
        }
        else
            ReturnPet();
    }
    private void ReturnPet()
    {
        IsComplete = true;
        TaskManager.Instance.petZones[currentTask.pet.petType].AssignEmployeeToQueue(this);
    }

    public void FinishTask()
    {
        currentTask = null;
        Debug.Log($"{name} completed the task and is now available.");
        if(!taskManager.AssignTaskFromQueue())
        {
            Patrol(cancellationTokenSource.Token);
        }    
    }

    public override async UniTask Patrol(CancellationToken _cancellationToken)
    {
        GraphNode randomNode;
        // For grid graphs
        var grid = AstarPath.active.data.gridGraph;
        randomNode = grid.nodes[Random.Range(0, grid.nodes.Length)];
        // Use the center of the node as the destination for example
        var destination1 = (Vector3)randomNode.position;

        _cancellationToken.ThrowIfCancellationRequested();

        await SetTarget(destination1);
        await UniTask.WaitForSeconds(3f);
        if (_cancellationToken.IsCancellationRequested)
        {
            return;
        }
        if (IsAvailable)
        {
            await Patrol(_cancellationToken);
        }    
    }
}
