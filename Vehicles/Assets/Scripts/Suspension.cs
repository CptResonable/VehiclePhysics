using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suspension : MonoBehaviour {
    Wheel wheel;

    Rigidbody rb;
    Rigidbody rbWheel;

    public float springRestLength; // Length of suspensionspring uncompressed or unextended.
    [SerializeField] float springTravelLength; // The length that the suspensionspring can compress or uncompress.
    [SerializeField] float springStiffness;
    [SerializeField] float damper;

    //Rigidbody rbCar;

    public float springMinLength; // Length of spring fully compressed.
    public float springMaxLength; // Length of spring fully extended.
    float springLength; // Current length of the spring;
    float springForce;
    float springVelocity;
    float damperForce;

    //Vector3 contactPoint;

    private void Awake() {
        //wheel = GetComponent<Wheel>();
        //rbCar = car.GetComponent<Rigidbody>();
        wheel = transform.GetChild(0).GetComponent<Wheel>();
        rb = transform.GetComponent<Rigidbody>();
        rbWheel = wheel.GetComponent<Rigidbody>();

        springMinLength = springRestLength - springTravelLength;
        springMaxLength = springRestLength + springTravelLength;
    }


    private void FixedUpdate() {
        float newSpringLength;
        newSpringLength = -wheel.transform.localPosition.y;

        springVelocity = (springLength - newSpringLength) / Time.deltaTime;
        springLength = newSpringLength;

        springForce = springStiffness * (springRestLength - springLength);
        damperForce = damper * springVelocity;

        rb.AddForceAtPosition(transform.up * (springForce / 2 + damperForce), transform.position);
        rbWheel.AddForceAtPosition(-transform.up * (springForce / 2 + damperForce), wheel.transform.position);
        //Debug.Log(springForce + damperForce);
        //rbWheel.velocity = Vector3.Scale(rbWheel.velocity, new Vector3(1, 0, 1));

        //SphereCollider collider = wheel.GetComponent<SphereCollider>();
        //contactPoint = collider.ClosestPoint(wheel.position);
        //contactMarker.transform.position = contactPoint;
    }
}
