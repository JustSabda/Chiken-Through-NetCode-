using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] GameObject[] tpPoint = new GameObject[2];
    [SerializeField] Vector3 offset;
    [SerializeField] bool isOnTp1;
    //[SerializeField] GameObject[] cam;
    bool isCanTp;
    private void Update()
    {
        #region Check the closest teleport
        if (FindClosestTP() == tpPoint[0])
            isOnTp1 = false;
        else
            isOnTp1 = true;
        #endregion

        Debug.Log("isCanTp :" + isCanTp);
        if (isCanTp && Input.GetKeyDown(KeyCode.Z))
        {
            isOnTp1 = !isOnTp1;
            CheckTeleport();
        }
    }

    void CheckTeleport()
    {
        if (isOnTp1)
        {
            this.transform.position = tpPoint[1].transform.position + offset;
            return;
        }
        else
        {
            this.transform.position = tpPoint[0].transform.position + offset;
            return;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WP"))
            isCanTp = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("WP"))
            isCanTp = false;
    }

    GameObject FindClosestTP()
    {
        float closestTP = Mathf.Infinity;
        GameObject closestTPObj = this.gameObject;

        foreach(var tp in tpPoint)
        {
            float distance = Vector3.Distance(this.transform.position, tp.transform.position);

            if(distance < closestTP)
            {
                closestTP = distance;
                closestTPObj = tp;
            }
        }

        return closestTPObj;
    }
}
