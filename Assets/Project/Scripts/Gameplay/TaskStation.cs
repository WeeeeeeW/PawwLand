using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskStation : BaseTaskStation
{
    private void Start()
    {
        entityQueue = new List<Entity>();
    }
    //public override IEnumerator AdvanceQueue(bool requestedService = false)
    //{
    //    Employee _employee = (Employee)entityQueue.Dequeue();
    //    //_employee.GetComponent<FollowerEntity>().enableLocalAvoidance = true;
    //    Debug.Log($"{_employee.employeeName} finished {serviceType} at {name}");
    //    advanceQueue?.Invoke();
    //    _employee.UnsubscribeToStation(this);
    //    yield return null;
    //}
}
