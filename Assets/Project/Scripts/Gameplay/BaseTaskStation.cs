using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public abstract class BaseTaskStation : MonoBehaviour
{
    //public string stationName;
    public ServiceType serviceType;
    public Transform entityTaskPosition;
    public Transform taskPosition, exitTF;
    public float taskDuration;
    protected List<Entity> entityQueue;
    public Action advanceQueue;
    public bool isAvailable;
    public virtual async UniTask QueueUp(Entity _entity)
    {
        entityQueue.Add(_entity);
        _entity.SetQueueTarget(entityTaskPosition.position + entityTaskPosition.forward * (entityQueue.Count - 1) * 2f);
        _entity.transform.forward = -entityTaskPosition.forward;
    }
    public virtual void AdvanceQueue(bool requestService = false)
    {
        entityQueue.RemoveAt(0);
        for (int i = 0; i < entityQueue.Count; i++)
        {
            var x = i;
            entityQueue[x].SetQueueTarget(entityTaskPosition.position + entityTaskPosition.forward * x * 2f);
        }
    }
}
