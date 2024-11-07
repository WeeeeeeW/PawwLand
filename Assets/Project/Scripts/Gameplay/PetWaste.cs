using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetWaste : MonoBehaviour
{
    public void CreateWaste(Transform zone)
    {
        TaskManager.Instance.CreateTask(new IdleTask(ServiceType.Clean, zone, gameObject));
    }

}
