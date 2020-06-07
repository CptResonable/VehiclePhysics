using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steering : MonoBehaviour {
    [SerializeField] float sensitivity;
    [SerializeField] float maxAngle;

    // Front wheels, used for steering.
    [SerializeField] Transform suspensionOrigin_FL;
    [SerializeField] Transform suspensionOrigin_FR;
    Transform wheel_FL;
    Transform wheel_FR;

    // Back wheels, not used for steering.
    [SerializeField] Transform suspensionOrigin_BL;
    [SerializeField] Transform suspensionOrigin_BR;
    Transform wheel_BL;
    Transform wheel_BR;

    [SerializeField] float angle;

    [SerializeField] float backToFrontLength;
    float leftToRightLength;

    Vector3 ghostWheelPosition;

    private void Awake() {
        wheel_FL = suspensionOrigin_FL.GetChild(0);
        wheel_FR = suspensionOrigin_FR.GetChild(0);
        wheel_BL = suspensionOrigin_BL.GetChild(0);
        wheel_BR = suspensionOrigin_BR.GetChild(0);

        backToFrontLength = Vector3.Distance(suspensionOrigin_BL.position, suspensionOrigin_FL.position);
        leftToRightLength = Vector3.Distance(suspensionOrigin_BL.position, suspensionOrigin_BR.position);
    }

    private void Update() {
        if (Input.GetKey(KeyCode.LeftArrow) && angle > maxAngle * -1)
            angle -= sensitivity * Time.deltaTime;
        if (Input.GetKey(KeyCode.RightArrow) && angle < maxAngle)
            angle += sensitivity * Time.deltaTime;
        if (!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow)) {
            angle = Mathf.Lerp(angle, 0, Time.deltaTime * 5);
        }
    }

    private void FixedUpdate() {
        if (angle == 0) {
            //suspensionOrigin_FL.localRotation = Quaternion.Euler(0, 0, 0);
            //suspensionOrigin_FR.localRotation = Quaternion.Euler(0, 0, 0);
            wheel_FL.localRotation = Quaternion.Euler(0, 0, 0);
            wheel_FR.localRotation = Quaternion.Euler(0, 0, 0);
            return;
        }

        //transform.position = Vector3.Lerp(wheel_FL.position, wheel_FR.position, 0.5f);
        //transform.localRotation = Quaternion.Euler(transform.localRotation.x, angle, transform.localRotation.z);

        // Calculate turn radius.
        float turnRadius = Mathf.Tan((90 - Mathf.Abs(angle)) * Mathf.Deg2Rad) * backToFrontLength;

        // Calculate angle for each wheel.
        float angle_L;
        float angle_R;
        if (angle > 0) {
            angle_L = Mathf.Atan((turnRadius + leftToRightLength / 2) / backToFrontLength) * Mathf.Rad2Deg;
            angle_L = 90 - angle_L;

            angle_R = Mathf.Atan((turnRadius - leftToRightLength / 2) / backToFrontLength) * Mathf.Rad2Deg;
            angle_R = 90 - angle_R;
        }
        else {
            angle_L = Mathf.Atan((turnRadius - leftToRightLength / 2) / backToFrontLength) * Mathf.Rad2Deg;
            angle_L = (90 - angle_L) * -1;

            angle_R = Mathf.Atan((turnRadius + leftToRightLength / 2) / backToFrontLength) * Mathf.Rad2Deg;
            angle_R = (90 - angle_R) * -1;
        }

        //suspensionOrigin_FL.localRotation = Quaternion.Euler(0, -angle_L, 0);
        //suspensionOrigin_FR.localRotation = Quaternion.Euler(0, -angle_R, 0);
        wheel_FL.localRotation = Quaternion.Euler(0, angle_L, 0);
        wheel_FR.localRotation = Quaternion.Euler(0, angle_R, 0);
    }
}
