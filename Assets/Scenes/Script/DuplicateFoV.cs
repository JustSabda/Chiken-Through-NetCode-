using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuplicateFoV : MonoBehaviour
{
    [SerializeField] GameObject FoVCone, circleFov;
    [SerializeField] GameObject FoVParent;
    [SerializeField] MeshRenderer fovRenderer, circleFovRenderer;
    [SerializeField] Material newFovMaterial;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("NewFoVDelay", .2f);
    }

    void NewFoVDelay()
    {
        GameObject newFoVCone = Instantiate(FoVCone, FoVParent.transform.position, transform.rotation);
        GameObject newFoVCircle = Instantiate(circleFov, FoVParent.transform.position, transform.rotation);
        newFoVCone.transform.SetParent(FoVParent.transform);
        newFoVCircle.transform.SetParent(FoVParent.transform);
        fovRenderer.material = newFovMaterial;
        circleFovRenderer.material = newFovMaterial;
        Debug.Log("Delay");
    }
}
