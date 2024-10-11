using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskStation : BaseTaskStation
{
    private void Start()
    {
        entityQueue = new Queue<Entity>();
    }
    public override IEnumerator AdvanceQueue(bool requestedService = false)
    {
        Employee _employee = (Employee)entityQueue.Dequeue();
        //_employee.GetComponent<FollowerEntity>().enableLocalAvoidance = true;
        Debug.Log($"{_employee.employeeName} finished {serviceType} at {name}");
        advanceQueue?.Invoke();
        _employee.UnsubscribeToStation(this);
        yield return null;
    }

    public override void QueueUp(Entity _entity)
    {
        Employee _employee = _entity.GetComponent<Employee>();
        //_employee.GetComponent<FollowerEntity>().enableLocalAvoidance = false;
        Debug.Log(entityQueue);
        entityQueue.Enqueue(_employee);
        for (int i = entityQueue.Count; i > 1; i--)
        {
            var x = i - 1;
            _employee.AddActionQueue(() => _employee.SetQueueTarget(employeeTaskPosition.transform.position + queueStart.forward * x * 1.5f));
        }
        //queueStart.transform.position = customerIn.transform.position + customerIn.forward * entityQueue.Count * 1.5f;
        _employee.AddActionQueue(() => _employee.SetQueueTarget(employeeTaskPosition));
        _employee.InvokeQueue();
    }
}
