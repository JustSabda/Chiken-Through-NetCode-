using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    Rigidbody rb;
    PlayerMovement movement = new PlayerMovement();
    [SerializeField] float speed;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        movement = this.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(HunterDash());
        }
    }

    IEnumerator HunterDash()
    {
        movement.speed = 2500;
        movement.runSpeed = 2500;
        yield return new WaitForSeconds(.3f);
        movement.speed = movement.initialSpeed;
        movement.runSpeed = movement.initialRunSpeed;
        StopAllCoroutines();
    }
}
