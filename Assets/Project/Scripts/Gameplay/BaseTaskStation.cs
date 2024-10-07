using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTaskStation : MonoBehaviour
{
    //public string stationName;
    public ServiceType serviceType;
    public Transform employeeTaskPosition;
    public Transform taskPosition;
    public float taskDuration;
    protected Queue<Entity> entityQueue;
    public abstract void QueueUp(Entity _entity);
    public abstract IEnumerator AdvanceQueue(bool requestService);

}
