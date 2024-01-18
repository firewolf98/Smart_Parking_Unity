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
    private float currentAccelleration = 0f;
    private float currentBreakingForce = 0f;
    private float currentTurnAngle = 0f;

    void FixedUpdate()
    {
        currentAccelleration = accelleration * Input.GetAxis("Vertical");
        if (Input.GetKey(KeyCode.Space))
            currentBreakingForce = breakingForce;
        else
            currentBreakingForce = 0f;

        frontRight.motorTorque = currentAccelleration;
        frontLeft.motorTorque = currentAccelleration;

        frontRight.brakeTorque = currentBreakingForce;
        frontLeft.brakeTorque = currentBreakingForce;
        backRight.brakeTorque = currentBreakingForce;
        backLeft.brakeTorque = currentBreakingForce;

        currentTurnAngle = maxturnAngle * Input.GetAxis("Horizontal");
        frontLeft.steerAngle = currentTurnAngle;
        frontRight.steerAngle = currentTurnAngle;


    }
}
