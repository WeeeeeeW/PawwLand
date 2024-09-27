using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField] private Transform petHolder;
    [SerializeField] IdleTask task;

    void AssignTask(IdleTask _task, Pet _pet)
    {
        task = _task;
        _pet.transform.parent = petHolder;
        _pet.transform.localPosition = Vector3.zero;
    }
}
