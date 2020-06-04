using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drive : MonoBehaviour {
    Rigidbody rb;
    float driveForce = 0;

    private void Awake() {
        rb = transform.GetComponent<Rigidbody>();
    }

    private void Update() {
        if (Input.GetKey(KeyCode.UpArrow))
            driveForce = 6000;
        else if (Input.GetKey(KeyCode.DownArrow))
            driveForce = -6000;
        else
            driveForce = 0;
    }
    private void FixedUpdate() {
        rb.AddForce(transform.right * driveForce);
    }
}
