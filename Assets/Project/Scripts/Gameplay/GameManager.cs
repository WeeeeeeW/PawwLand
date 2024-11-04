using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public PatrolArea patrolArea;
    public AstarPath astarPath;
    [SerializeField] LockedZone[] lockedZones;
    private void Awake()
    {
        base.Awake();
        foreach (var zone in lockedZones)
        {
            zone.Init();
        }
        astarPath.Scan();
    }
    public Vector3 RandomPatrolPosition()
    {
        return new Vector3(Random.Range(patrolArea.corners[0].position.x, patrolArea.corners[1].position.x),
                           Random.Range(patrolArea.corners[0].position.y, patrolArea.corners[1].position.y),
                           Random.Range(patrolArea.corners[0].position.z, patrolArea.corners[1].position.z));
    }
}
