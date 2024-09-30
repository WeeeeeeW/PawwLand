using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : MonoBehaviour
{
    [SerializeField] Manager[] managers;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var manager in managers)
            manager.AssignToCounter(this);
    }
}
