using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentPlayer : MonoBehaviour
{

    [HideInInspector] public bool Destroyed;
    private void Awake()
    {
        Destroyed = false;
    }
    void Update()
    {
        if (Destroyed)
        {
            Destroy(this.gameObject);
        }
    }
}
