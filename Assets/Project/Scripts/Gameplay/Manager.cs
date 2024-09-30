using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Manager : Entity
{
    [SerializeField] private Transform petHolder;
    [SerializeField] ServiceType service;
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    public void AssignTask(ServiceType _task, Pet _pet)
    {
        service = _task;
        _pet.transform.parent = petHolder;
        _pet.transform.localPosition = Vector3.zero;
        SetTarget(TaskManager.Instance.cageDropoff);
    }
}
