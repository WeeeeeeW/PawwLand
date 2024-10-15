using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TaskStation : MonoBehaviour
{
    public Transform EntityTarget;
    private QueueManager queueManager;
    [SerializeField] List<Transform> queuePos;
    private void Start()
    {
        queueManager = new QueueManager(queuePos);
    }
}
