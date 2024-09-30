using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var entity = other.GetComponent<Entity>();
        if(entity != null)
        {
            if(entity.destination == transform)
                entity.ReachDestination();
        }
    }
}
