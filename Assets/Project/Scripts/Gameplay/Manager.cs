using DG.Tweening.Core.Easing;
using Pathfinding;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Manager : Entity
{
    [SerializeField] private Transform petHolder;
    [SerializeField][ReadOnly] Counter counter;
    private TaskManager taskManager;
    public bool isAvailable;
    public float efficiency = 1;
    private void Awake()
    {
        navMeshAgent = GetComponent<FollowerEntity>();
        actionQueue = new Queue<Action>();
    }
    private void Start()
    {
        taskManager = TaskManager.Instance;
        isAvailable = true;
    }
    public void AssignToCounter(Counter _counter)
    {
        counter = _counter;
    }

    public void AssignTask(ServiceType _service, Pet _pet)
    {
        isAvailable = false;
        _pet.transform.parent = petHolder;
        _pet.transform.localPosition = Vector3.zero;
        SetTarget(taskManager.petzone.dropoff);
        actionQueue.Enqueue(() => StartCoroutine(DropPetIntoPetZone(_service, _pet)));
    }
    public IEnumerator DropPetIntoPetZone(ServiceType _service, Pet _pet)
    {
        yield return new WaitForSeconds(.2f);
        taskManager.AssignPetToZone(_pet);
        taskManager.CreateTask(_pet.owner, _pet, _service);
        SetTarget(taskManager.employeeCounter);
        actionQueue.Enqueue(() => StartCoroutine(CorrectRotationToCounter()));
    }
    IEnumerator CorrectRotationToCounter()
    {
        yield return new WaitForSeconds(.1f);
        Vector3 startRotation = transform.forward;
        float rotationSpeed = 600f;
        Vector3 desiredRotation = counter.transform.position - transform.position;
        desiredRotation.y = 0;
        float rotationAngle = Vector3.Angle(transform.forward, desiredRotation);
        float rotationDuration = rotationAngle / rotationSpeed;
        float elapsedTime = 0f;

        while (elapsedTime < rotationDuration)
        {
            elapsedTime += Time.deltaTime;
            transform.forward = Vector3.Lerp(startRotation, desiredRotation, elapsedTime / rotationDuration);
            yield return null;
        }
        isAvailable = true;
    }

}
