using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{

    private float speed = 1f;
    private float torque = 1f;
    private float minSpeedBeforeTorque = 0.3f;
    private float minSpeedBeforeIdle = 0.2f;
    private Animator carAnimator;
    public Direction CurrentDirection { get; set; } = Direction.Idle;
    public bool isAutonomous {  get; set; } = false;
    private Rigidbody carRigidBody;

    public enum Direction
    {
        Idle,
        MoveForward,
        MoveBackward,
        TurnLeft,
        TurnRight
    }

    void Awake()
    {
        carRigidBody = GetComponent<Rigidbody>();    
    }
    void Update()
    {
        if (carRigidBody.velocity.magnitude <=minSpeedBeforeIdle)
        {
            CurrentDirection = Direction.Idle;
            ApplyAnimatorState(Direction.Idle);
        }
    }

    void FixedUpdate() => ApplyMovement();

    public void ApplyMovement()
    {
        if(Input.GetKey(KeyCode.UpArrow) || (CurrentDirection==Direction.MoveForward && isAutonomous)) 
        {
            ApplyAnimatorState(Direction.MoveForward);
            carRigidBody.AddForce(transform.forward * speed, ForceMode.VelocityChange);
        }

        if (Input.GetKey(KeyCode.DownArrow) || (CurrentDirection == Direction.MoveBackward && isAutonomous))
        {
            ApplyAnimatorState(Direction.MoveBackward);
            carRigidBody.AddForce(transform.forward * speed, ForceMode.VelocityChange);
        }

        if (Input.GetKey(KeyCode.LeftArrow) && canApplyTorque() || (CurrentDirection == Direction.TurnLeft && isAutonomous))
        {
            ApplyAnimatorState(Direction.TurnLeft);
            carRigidBody.AddTorque(transform.up * -torque);
        }

        if (Input.GetKey(KeyCode.RightArrow) && canApplyTorque() || (CurrentDirection == Direction.TurnRight && isAutonomous))
        {
            ApplyAnimatorState(Direction.TurnRight);
            carRigidBody.AddTorque(transform.up * torque);
        }
    }

    void ApplyAnimatorState(Direction direction)
    {
        carAnimator.SetBool(direction.ToString(), true);

        switch(direction)
        {
            case Direction.Idle:
                carAnimator.SetBool(Direction.MoveBackward.ToString(), false);
                carAnimator.SetBool(Direction.MoveForward.ToString(), false);
                carAnimator.SetBool(Direction.TurnLeft.ToString(), false);
                carAnimator.SetBool(Direction.TurnRight.ToString(), false);
                break;
            case Direction.MoveForward:
                carAnimator.SetBool(Direction.Idle.ToString(), false);
                carAnimator.SetBool(Direction.MoveBackward.ToString(), false);
                carAnimator.SetBool(Direction.TurnLeft.ToString(), false);
                carAnimator.SetBool(Direction.TurnRight.ToString(), false);
                break;
            case Direction.MoveBackward:
                carAnimator.SetBool(Direction.Idle.ToString(), false);
                carAnimator.SetBool(Direction.MoveForward.ToString(), false);
                carAnimator.SetBool(Direction.TurnLeft.ToString(), false);
                carAnimator.SetBool(Direction.TurnRight.ToString(), false);
                break;
            case Direction.TurnLeft:
                carAnimator.SetBool(Direction.TurnRight.ToString(), false);
                break;
            case Direction.TurnRight:
                carAnimator.SetBool(Direction.TurnLeft.ToString(), false);
                break;
        }
    }

    public bool canApplyTorque()
    {
        Vector3 velocity = carRigidBody.velocity;
        return Mathf.Abs(velocity.x) >= minSpeedBeforeTorque || Mathf.Abs(velocity.z) >= minSpeedBeforeTorque;
    }
}
