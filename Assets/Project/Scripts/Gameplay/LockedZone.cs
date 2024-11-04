using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
public class LockedZone : MonoBehaviour
{
    [SerializeField] Renderer Renderer;
    [SerializeField] GameObject[] hiddenGO;
    public void Init()
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
    [Button]
    void Test()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
            child.DOPunchPosition(child.up * 2f, .1f, elasticity: 0, vibrato: 0);
            child.DOPunchScale(new Vector3(-1.5f, 1.5f, -1.5f) * .1f, 1f, elasticity: 1, vibrato: 4);
        }
    }
    private void UnlockZone()
    {
        Debug.Log("Zone unlocked");
        Renderer.enabled = false;
        GetComponent<Collider>().enabled = false;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
            child.DOPunchPosition(child.up * 2f, .1f, elasticity: 0, vibrato: 0);
            child.DOPunchScale(new Vector3(-1.5f, 1.5f, -1.5f) * .1f, 1f,elasticity: 1, vibrato: 4);
        }
        GameManager.Instance.astarPath.Scan();
    }
}
