using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core.Easing;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Employee : Entity
{
    public IdleTask currentTask;
    private Queue<IdleTask> tasks;
    public bool IsAvailable => currentTask == null;
    public bool IsComplete = false;

    [SerializeField] FaceCamera progressCanvas;
    [SerializeField] Image progressFill;
    protected override void Start()
    {
        base.Start();
        Patrol();
    }
    public void TakeTask(IdleTask _task)
    {
        StopCoroutine("PatrolCoroutine");
        IsComplete = false;
        tasks = new Queue<IdleTask>();
        switch (_task.serviceType)
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
        TaskManager.Instance.taskStations[currentTask.serviceType][0].AssignEmployeeToQueue(this);
    }

    public void ShowProgress(float _duration)
    {
        progressCanvas.gameObject.SetActive(true);
        progressFill.fillAmount = 0;
        progressFill.DOFillAmount(1, _duration).OnComplete(() => progressCanvas.gameObject.SetActive(false));
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
        if (!taskManager.AssignTaskFromQueue())
        {
            Patrol();
        }
    }

    public override async void Patrol()
    {
       StartCoroutine("PatrolCoroutine");
    }

    private IEnumerator PatrolCoroutine()
    {
        GraphNode randomNode;
        // For grid graphs
        var grid = AstarPath.active.data.gridGraph;
        randomNode = grid.nodes[Random.Range(0, grid.nodes.Length)];
        // Use the center of the node as the destination for example
        var destination1 = (Vector3)randomNode.position;

        UniTask task = SetTarget(destination1);
        yield return new WaitUntil(() => task.Status.IsCompleted());
        yield return new WaitForSeconds(3f);
        if (IsAvailable)
        {
            Patrol();
        }
        yield return null;
    }    
}
