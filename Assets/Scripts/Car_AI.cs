using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

namespace UnityStandardAssets.Vehicles.Car {
public class Car_AI : Agent
{
    public float distance = 3f;
    public float carSpeed = 5f;
    public bool findParking = true;
    private Rigidbody rb;
    private bool isLookingForPark;
    private bool isParking;
    private Vector3 firstPosition;
    private Quaternion firstRotation;
    private Vector3 finalPosition;
    private int steps = 0;

    public GameObject target;
    private bool inTarget = false;

    private CarController carController;
    private RayPerceptionSensorComponent3D RayPerceptionSensorComponent;
    EnvironmentParameters defaultParameters;
    private Vector3 detectedSpotLocation;
    private float predictedSpotSize = 0f;


    public float envRadiusX = 3f;
    public float envRadiusZ = 10f;
    
    /*private void Update()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, transform.forward, out hit, distance);

        Vector3 posizioneOrigine = transform.position;
        Vector3 direzione = transform.forward;

        if (Physics.Raycast(posizioneOrigine, direzione, out hit, distance))
        {
            Debug.DrawRay(posizioneOrigine, direzione * distance, Color.red);
            Debug.Log("Il raycast ha colpito l'oggetto con il tag: " + hit.collider.tag);
        }
        else
        {
            Debug.DrawRay(posizioneOrigine, direzione * distance, Color.green);
        }

        if (Physics.Raycast(posizioneOrigine, direzione * distance, out hit, distance))
        {
            if (hit.collider.CompareTag("Grass"))
            {
                Stop();
            }
            else
            {
                //Move();
            }
        }
        else
        {
            //Move();
        }
    }

    void Stop()
    {
        transform.position += new Vector3(0, 0, 0);
    }

    void Move()
    {
        transform.position -= new Vector3(carSpeed * Time.deltaTime, 0, 0);
    }

    */

    // TO DO: CHANGE envRAdius
    void FixedUpdate(){
        if(isLookingForPark){
            CruiseControl(4f);
            FindParkingSpot();
        }

        if(isParking && findParking){
            PositionCar(-1f);
        }
        if(!isLookingForPark){
            RequestDecision();
            if(Mathf.Abs(transform.position.x - target.transform.position.x) > envRadiusX || Mathf.Abs(transform.position.z - target.transform.position.z) > envRadiusZ){
                AddReward(-100f);
                EndEpisode();
            }
        }  
    }

    private void Reset()
    {
        if (findParking)
        {
            isLookingForPark = true;
            isParking = false;
        }

        rb.transform.position = firstPosition;
        rb.transform.rotation = firstRotation;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        steps = 0;
    }

    public override void Initialize(){
        carController = GetComponent<CarController>();
        rb = GetComponent<Rigidbody>();

        isLookingForPark = findParking;

        RayPerceptionSensorComponent = GetComponent<RayPerceptionSensorComponent3D>();

        defaultParameters = Academy.Instance.EnvironmentParameters; //TENERLI?!
        
        firstPosition = transform.position;
        firstRotation = transform.rotation;
        
        finalPosition = firstPosition;

        Reset();
    }

    private void FindParkingSpot(){
        var RpMeasurements = RayPerceptionMeasurements();
        
        int LeftLikelihoodScore = 0;
        int RightLikelihoodScore = 0;
        if(RpMeasurements.RDistL[2] > 0.9f){
            LeftLikelihoodScore += 1;
        }
        if(RpMeasurements.RDistR[2] > 0.9f){
            RightLikelihoodScore += 1;
        }
        if(RpMeasurements.RDistL.Sum() < RpMeasurements.RDistR.Sum()){
            LeftLikelihoodScore += 1;
        }
        else{
            RightLikelihoodScore += 1;
        }
        float RayDiff1 = Mathf.Abs(RpMeasurements.RDistL[0] - RpMeasurements.RDistL[4]);
        float RayDiff2 = Mathf.Abs(RpMeasurements.RDistL[1] - RpMeasurements.RDistL[3]);
        float TotalRayDiff = RayDiff1 + RayDiff2;

        if(TotalRayDiff < 0.1f){
            LeftLikelihoodScore += 1;
        }

        RayDiff1 = Mathf.Abs(RpMeasurements.RDistR[0] - RpMeasurements.RDistR[4]);
        RayDiff2 = Mathf.Abs(RpMeasurements.RDistR[1] - RpMeasurements.RDistR[3]);
        TotalRayDiff = RayDiff1 + RayDiff2;

        if(TotalRayDiff < 0.1f){
            RightLikelihoodScore += 1;
        }

        if(LeftLikelihoodScore == 3){
            float PredictedSpace = (RpMeasurements.RDistL[1] * Mathf.Cos(60*Mathf.Deg2Rad)) + (RpMeasurements.RDistL[3] * Mathf.Cos(60*Mathf.Deg2Rad));
            PredictedSpace *= 7;
            

            if(PredictedSpace > 3f){
                isLookingForPark = false;
                isParking = true;
                predictedSpotSize = PredictedSpace;
                detectedSpotLocation = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                Debug.Log("Found spot left");
            }
        }
        else if(RightLikelihoodScore == 3){
            // Validate the spot is lage enough
            float PredictedSpace = (RpMeasurements.RDistR[1] * Mathf.Cos(60*Mathf.Deg2Rad)) + (RpMeasurements.RDistR[3] * Mathf.Cos(60*Mathf.Deg2Rad));
            // Distances are in normalised units, so multiply by the ray length to get the actual distance
            PredictedSpace *= 7;
            
            if(PredictedSpace > 3f){
                isLookingForPark = false;
                isParking = true;
                predictedSpotSize = PredictedSpace;
                detectedSpotLocation = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                Debug.Log("Found spot right");
            }
        }

        else{
            isLookingForPark = true;
        }

    }

    private (float[] RDistL, float[] RDistR, float RDistF, float RDistB) RayPerceptionMeasurements(bool LogValues = false){
        RayPerceptionInput RayPerceptionIn = RayPerceptionSensorComponent.GetRayPerceptionInput();
        RayPerceptionOutput RayPerceptionOut = RayPerceptionSensor.Perceive(RayPerceptionIn);
        RayPerceptionOutput.RayOutput[] RayOutputs = RayPerceptionOut.RayOutputs;
        int RayAmount = RayOutputs.Length - 1;
        float[] RayDistances = new float[RayAmount - 1];
        float[] RayDistancesLeft = new float[(RayAmount - 2) / 2];
        float[] RayDistancesRight = new float[(RayAmount - 2) / 2];

        float RayDistanceFront = RayOutputs[0].HitFraction;
        float RayDistanceBack = RayOutputs[RayAmount - 1].HitFraction;

        for(int i = 1; i < RayAmount-1; i++)
        {
            if(i % 2 == 0){
                RayDistancesLeft[(i/2)-1] = RayOutputs[i].HitFraction;
            }
            else{
                RayDistancesRight[(i-1)/2] = RayOutputs[i].HitFraction;
            }
        }

        return (RayDistancesLeft, RayDistancesRight, RayDistanceFront, RayDistanceBack);
    }
    
    private void CruiseControl(float Speed){
        if(carController.CurrentSpeed < Speed){
            carController.Move(0, 0.5f, 0f, 0f);
        }
        else if(carController.CurrentSpeed > Speed){
            carController.Move(0, -0.5f, 0f, 0f);
        }
    }

    private void PositionCar(float offsetX){
        float coveredX = Mathf.Abs(transform.position.x - detectedSpotLocation.x);
        float absoluteOffsetX = Mathf.Abs(offsetX);

        if(coveredX < absoluteOffsetX && offsetX < 0){
            carController.Move(0f, -.3f, 0f, 0f);
        }
        else if(coveredX < absoluteOffsetX && offsetX > 0){
            carController.Move(0f, .1f, 0f, 0f);
        }
        else{
            isParking = false;
        }
    }

    private float CalculateReward(){
        float reward = 0f;

        float totDirectionChangeReward = 0f;
        float totAngleChangeReward = 0f;
        float totDistanceReward = 0f;

        if(finalPosition != Vector3.zero){
            float distanceToTargetX = Mathf.Abs(transform.position.x - target.transform.position.x);
            float distanceToTargetZ = Mathf.Abs(transform.position.z - target.transform.position.z);

            float lastDistanceToTargetX = Mathf.Abs(finalPosition.x - target.transform.position.x);
            float lastDistanceToTargetZ = Mathf.Abs(finalPosition.z - target.transform.position.z);

            float directionChangeX = lastDistanceToTargetX - distanceToTargetX;
            float directionChangeZ = lastDistanceToTargetZ - distanceToTargetZ;

            totDirectionChangeReward = (directionChangeX + directionChangeZ) * 10f;
            totDirectionChangeReward = Mathf.Clamp(totDirectionChangeReward, -0.5f, 0.5f);

            float distanceRewardX = (1f - distanceToTargetX/envRadiusX);
            float distanceRewardZ = (1f - distanceToTargetZ/envRadiusZ);

            totDistanceReward = (distanceRewardX + distanceRewardZ) / 20f;

            reward += totDirectionChangeReward + totDistanceReward;
        }

        if(inTarget){
            float angleToTarget = Vector3.Angle(transform.forward, target.transform.forward);
            if(angleToTarget > 90f){
                angleToTarget = 180f - angleToTarget;
            }

            angleToTarget = Mathf.Clamp(angleToTarget, 0f, 90f);
            float angleReward = (-(1f/45f) * angleToTarget) + 1f;

            totAngleChangeReward = angleReward + 1f;
            reward += totAngleChangeReward;

            float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

            if(angleToTarget < 2.5f && distanceToTarget < 1f && Mathf.Abs(carController.CurrentSpeed) < 2f){
                Debug.Log("Car parked!");
                reward += 100f;
                EndEpisode();
            }

        }

        finalPosition = transform.position;
        return reward;            
    }

    public override void OnEpisodeBegin(){
        Reset();
    }

     public override void OnActionReceived(ActionBuffers actions){
        float steering = actions.ContinuousActions[0];
        float accel = actions.ContinuousActions[1];
        float reverse = actions.ContinuousActions[2];

        accel = (accel + 1) / 2;
        reverse = (reverse + 1) / 2;

        accel = accel - reverse;
        
        if(!isLookingForPark){
            carController.Move(steering, accel, 0f, 0f);
        }
        
        steps++;

        float reward = CalculateReward();
        AddReward(reward);
    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        ActionSegment<float> continuousActionsOut = actionsOut.ContinuousActions;

        float steering = Input.GetAxis("Horizontal");
        float accel = Input.GetAxis("Accelerate");  
        float reverse = Input.GetAxis("Reverse");   

        accel = accel * 2 - 1;
        reverse = reverse * 2 - 1;

        continuousActionsOut[0] = steering;
        continuousActionsOut[1] = accel;
        continuousActionsOut[2] = reverse;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Finish"){
            inTarget = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Finish"){
            inTarget = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject.tag);
        if (collision.gameObject.tag == "Grass")
        {
            AddReward(-10f);
            EndEpisode();
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Gate")
        {
            AddReward(-2f);
        }
        else if(collision.gameObject.tag == "Car")
        {
            
            float reward = -Mathf.Abs(carController.CurrentSpeed) * 50f - 5f;
            AddReward(reward);
            EndEpisode();
        }
    }
}
}
