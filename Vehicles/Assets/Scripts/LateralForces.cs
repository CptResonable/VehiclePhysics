using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LateralForces : MonoBehaviour {
    [SerializeField] Transform car;
    [SerializeField] float strength;

    //[SerializeField] bool log = false;
    //Transform wheelTransform;
    Rigidbody rb;

    //Rigidbody rbCar;
    Wheel wheel;

    Vector3 cpVelocity;
    Vector3 localCpVelocity;



    private void Awake() {
        //rbCar = car.GetComponent<Rigidbody>();
        wheel = GetComponent<Wheel>();
        rb = GetComponent<Rigidbody>();
        //wheelTransform = transform.GetChild(0);
    }

    private void FixedUpdate() {
        //if (!wheel.onGround)
        //    return;

        //cpVelocity = rbCar.GetPointVelocity(wheel.contactPoint);
        cpVelocity = rb.GetPointVelocity(transform.position);
        localCpVelocity = transform.InverseTransformDirection(cpVelocity);
        //Debug.Log(localCpVelocity.z);

        LeftRight();
        //ForwardBackward();
    }

    void LeftRight() {
        //Debug.Log(wheel.collisionDetector.contacts.Count);
        if (wheel.collisionDetector.contacts.Count > 0)
            rb.AddForceAtPosition(-transform.forward * localCpVelocity.z * strength, transform.position);
    }
}