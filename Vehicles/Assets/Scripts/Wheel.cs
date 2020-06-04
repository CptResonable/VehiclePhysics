using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour {
    [SerializeField] float radius;

    Rigidbody rb;
    Transform contactMarker;
    List<Collider> overlappingColliders = new List<Collider>();
    Vector3 closestPoint;
    float closestDst;

    [SerializeField] float springRestLength; // Length of suspensionspring uncompressed or unextended.
    [SerializeField] float springTravelLength; // The length that the suspensionspring can compress or uncompress.
    [SerializeField] float springStiffness;
    [SerializeField] float damper;
    public float springMinLength; // Length of spring fully compressed.
    public float springMaxLength; // Length of spring fully extended.
    float springLength; // Current length of the spring;
    float springForce;
    float springVelocity;
    float damperForce;

    private void Awake() {
        contactMarker = transform.Find("ContactMarker");
        rb = GetComponent<Rigidbody>();

        springMinLength = springRestLength - springTravelLength;
        springMaxLength = springRestLength + springTravelLength;
    }

    private void OnTriggerEnter(Collider other) {
        overlappingColliders.Add(other);
    }

    private void OnTriggerExit(Collider other) {
        overlappingColliders.Remove(other);
    }

    private void FixedUpdate() {
        //Debug.Log(overlappingColliders.Count);
        closestDst = 9000; 
        foreach (Collider collider in overlappingColliders) {
            Vector3 point = collider.ClosestPointOnBounds(transform.position);
            float dst = Vector3.Distance(transform.position, point);
            if (dst < closestDst) {
                closestDst = dst;
                closestPoint = point;
            }
        }
        contactMarker.position = closestPoint;

        if (closestDst != 9000)
            NormalForce();
    }

    void NormalForce() {
        Vector3 normal = transform.position - closestPoint;

        float newSpringLength;
        newSpringLength = radius - normal.magnitude;

        Debug.Log(newSpringLength);

        springVelocity = (springLength - newSpringLength) / Time.deltaTime;
        springLength = newSpringLength;

        springForce = springStiffness * (springRestLength - springLength);
        damperForce = damper * springVelocity;

        rb.AddForceAtPosition(-normal * (springForce + damperForce), closestPoint);
    }
}
