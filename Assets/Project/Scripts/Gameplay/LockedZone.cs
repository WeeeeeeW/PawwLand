using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Diagnostics.Tracing;
public class LockedZone : MonoBehaviour
{
    [SerializeField] Renderer Renderer;
    [SerializeField] GameObject[] hiddenGO;
    private void Start()
    {
        foreach (GameObject go in hiddenGO)
        {
            go.SetActive(false);
        }
    }
    private void OnMouseUpAsButton()
    {
        UnlockZone();
    }

    private void UnlockZone()
    {
        Debug.Log("Zone unlocked");
        Renderer.enabled = false;
        GetComponent<Collider>().enabled = false;
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(true);
            child.DOPunchPosition(child.up * .05f, .3f,elasticity: 0);
            child.DOPunchScale(Vector3.one * .1f, .3f,elasticity: 0);
        }    
        GameManager.Instance.astarPath.Scan();
    }
}
