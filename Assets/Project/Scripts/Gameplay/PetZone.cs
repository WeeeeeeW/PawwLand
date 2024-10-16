using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PetZone : MonoBehaviour
{
    public Transform CashierDropoffTF;
    float taskDuration = .2f;

    public async UniTask ReceiveTask(IdleTask _idleTask)
    {
        await UniTask.WaitForSeconds(taskDuration);
        _idleTask.pet.transform.parent = transform;
        _idleTask.pet.transform.localPosition = Vector3.zero;
        TaskManager.Instance.CreateTask(_idleTask);
    }
}
