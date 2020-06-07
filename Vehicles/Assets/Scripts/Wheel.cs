using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour {
    [SerializeField] float radius;

    Rigidbody rb;
    [HideInInspector] public CollisionDetector collisionDetector;
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
        collisionDetector = transform.Find("Collision detector").GetComponent<CollisionDetector>();
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
        //contactMarker.position = closestPoint;

        if (closestDst != 9000)
            NormalForce();
    }

    void NormalForce() {
        // Collider overlapps for each contact point.
        List<float> x = new List<float>();
        List<float> y = new List<float>();
        // Overlapps added.
        float xTotal = 0;
        float yTotal = 0;
        // Force distribution.
        List<float> xForces = new List<float>();
        List<float> yForces = new List<float>();

        foreach (ContactPoint contact in collisionDetector.contacts) {
            Vector3 point_ls = transform.InverseTransformPoint(contact.point);
            x.Add(point_ls.x);
            xTotal += point_ls.x;
            y.Add(point_ls.y);
            yTotal += point_ls.y;
        }

        for (int i = 0; i < x.Count; i++) {
            //xForces /= xTotal;

            Vector3 dirCenterToPoint = collisionDetector.contacts[i].point - transform.position;

            RaycastHit hit;
            if (collisionDetector.contacts[i].otherCollider.Raycast(new Ray(transform.position, dirCenterToPoint), out hit, radius)) {
                Vector3 normal = -dirCenterToPoint.normalized;

                float newSpringLength;
                newSpringLength = radius - hit.distance;
                //newSpringLength = radius - normal.magnitude;

                //Debug.Log(newSpringLength);

                springVelocity = (springLength - newSpringLength) / Time.deltaTime;
                springLength = newSpringLength;

                springForce = springStiffness * (springRestLength - springLength);
                damperForce = damper * springVelocity;

                rb.AddForceAtPosition(-collisionDetector.contacts[i].normal * (springForce + damperForce), closestPoint);
            }
        }


        //Vector3 dirCenterToPoint = collisionDetector.contacts[0].point - transform.position;

        //RaycastHit hit;
        //if (collisionDetector.contacts[0].otherCollider.Raycast(new Ray(transform.position, dirCenterToPoint), out hit, radius)) {
        //    Vector3 normal = -dirCenterToPoint.normalized;

        //    float newSpringLength;
        //    newSpringLength = radius - hit.distance;
        //    //newSpringLength = radius - normal.magnitude;

        //    //Debug.Log(newSpringLength);

        //    springVelocity = (springLength - newSpringLength) / Time.deltaTime;
        //    springLength = newSpringLength;

        //    springForce = springStiffness * (springRestLength - springLength);
        //    damperForce = damper * springVelocity;

        //    rb.AddForceAtPosition(-normal * (springForce + damperForce), closestPoint);
        //}
    }

    //void NormalForce() {
    //    Vector3 normal = transform.position - closestPoint;

    //    float newSpringLength;
    //    newSpringLength = radius - normal.magnitude;

    //    //Debug.Log(newSpringLength);

    //    springVelocity = (springLength - newSpringLength) / Time.deltaTime;
    //    springLength = newSpringLength;

    //    springForce = springStiffness * (springRestLength - springLength);
    //    damperForce = damper * springVelocity;

    //    rb.AddForceAtPosition(-normal * (springForce + damperForce), closestPoint);
    //}
}
