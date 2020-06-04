using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour {

    [SerializeField] Transform car;
    [SerializeField] Transform targetPosition;
    [SerializeField] Transform targetRotation;
    [SerializeField] float followSpeed;

    void Update() {
        transform.position = Vector3.Lerp(transform.position, targetPosition.position, followSpeed * Time.deltaTime);
        transform.LookAt(car);
    }
}
