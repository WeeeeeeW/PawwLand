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

        // Assign queue position based on current size of queue
        int queueIndex = waitingQueue.Count - 1;
        if (queueIndex < queuePositions.Count)
        {
            Vector3 targetQueuePosition = queuePositions[queueIndex].position;
            entity.SetTarget(targetQueuePosition);
        }
    }

    private void UpdateQueuePositions()
    {
        // Update each customer to move to the next position in the queue
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
}