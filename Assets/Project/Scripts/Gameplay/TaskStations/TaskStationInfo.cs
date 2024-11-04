using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Station", menuName = "GameAssets/Station",order = 0)]
public class TaskStationInfo : ScriptableObject
{
    public string StationName;
    public ServiceType ServiceType;
    public float BaseDuration;
}
