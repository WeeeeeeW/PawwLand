using System.Collections.Generic;
using UnityEngine;

public class QueueManager
{
    private Queue<Entity> waitingQueue = new Queue<Entity>();
    private List<Transform> queuePositions = new List<Transform>();  // Positions where customers will wait
    private bool isBusy = false;

    public QueueManager(List<Transform> queuePositions)
    {
        this.queuePositions = queuePositions;
    }

    public void AddToQueue(Entity entity)
    {
        waitingQueue.Enqueue(entity);
        Debug.Log($"{entity} added to the queue.");
        UpdateQueuePositions();
    }

    public void RemoveFromQueue()
    {
        if (waitingQueue.Count > 0)
        {
            Entity entity = waitingQueue.Dequeue();
            Debug.Log($"{entity} leaves the queue.");
            UpdateQueuePositions();
        }
    }

    private void UpdateQueuePositions()
    {
        // Update each entity to move to the next position in the queue
        int index = 0;
        foreach (Entity entity in waitingQueue)
        {
            if (index < queuePositions.Count)
            {
                Vector3 targetQueuePosition = queuePositions[index].position;
                entity.SetTarget(targetQueuePosition);
                index++;
            }
        }
    }
    public bool HasWaitingEntities()
    {
        return waitingQueue.Count > 0;
    }

    public Entity GetNextInQueue()
    {
        if (waitingQueue.Count > 0)
        {
            return waitingQueue.Peek();
        }
        return null;
    }
}