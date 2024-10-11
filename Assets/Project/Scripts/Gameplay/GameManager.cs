using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public PatrolArea patrolArea;

    public Vector3 RandomPatrolPosition()
    {
        return new Vector3(Random.Range(patrolArea.corners[0].position.x, patrolArea.corners[1].position.x),
                           Random.Range(patrolArea.corners[0].position.y, patrolArea.corners[1].position.y),
                           Random.Range(patrolArea.corners[0].position.z, patrolArea.corners[1].position.z));
    }
}
