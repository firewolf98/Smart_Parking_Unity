using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : MonoBehaviour
{
    [SerializeField] WheelCollider frontRight;
    [SerializeField] WheelCollider frontLeft;
    [SerializeField] WheelCollider backRight;
    [SerializeField] WheelCollider backLeft;
    public float accelleration = 500f;
    public float breakingForce = 300f;
    public float maxturnAngle = 15f;
    private float currentBreakingForce = 0f;
    private float currentTurnAngle = 0f;

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Space))
            currentBreakingForce = breakingForce;
        else
            currentBreakingForce = 0f;

        frontRight.motorTorque = accelleration;
        frontLeft.motorTorque = accelleration;

        frontRight.brakeTorque = currentBreakingForce;
        frontLeft.brakeTorque = currentBreakingForce;
        backRight.brakeTorque = currentBreakingForce;
        backLeft.brakeTorque = currentBreakingForce;

        currentTurnAngle = maxturnAngle * Input.GetAxis("Horizontal");
        frontLeft.steerAngle = currentTurnAngle;
        frontRight.steerAngle = currentTurnAngle;


    }
}
