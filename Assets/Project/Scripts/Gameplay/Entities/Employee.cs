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

    Coroutine _patrolCoroutine;
    protected override void Start()
    {
        base.Start();
        Patrol(GameManager.Instance.patrolArea.corners);
    }
    public void TakeTask(IdleTask _task)
    {
        if(_patrolCoroutine != null)
            StopCoroutine(_patrolCoroutine);
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
        TaskManager.Instance.petZones[currentTask.pet.PetType].AssignEmployeeToQueue(this);
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
        TaskManager.Instance.petZones[currentTask.pet.PetType].AssignEmployeeToQueue(this);
    }

    public void FinishTask()
    {
        currentTask = null;
        Debug.Log($"{name} completed the task and is now available.");
        if (!TaskManager.AssignTaskFromQueue())
        {
            Patrol(GameManager.Instance.patrolArea.corners);
        }
    }

    public override async void Patrol(Transform[] patrolArea)
    {
        _patrolCoroutine = StartCoroutine(PatrolCoroutine(patrolArea));
    }

    private IEnumerator PatrolCoroutine(Transform[] patrolArea)
    {
        GraphNode randomNode;
        // For grid graphs
        // Use the center of the node as the destination for example
        var destination1 = new Vector3(Random.Range(patrolArea[0].position.x, patrolArea[1].position.x), 0, Random.Range(patrolArea[0].position.z, patrolArea[1].position.z));

        UniTask task = SetTarget(destination1);
        yield return new WaitUntil(() => task.Status.IsCompleted());
        yield return new WaitForSeconds(3f);
        if (IsAvailable)
        {
            Patrol(patrolArea);
        }
        yield return null;
    }

    public override void OnEnterQueue()
    {
        //throw new System.NotImplementedException();
    }
}
