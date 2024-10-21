using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private void Update()
    {
        RotateToCamera();
    }
    void RotateToCamera()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
}
