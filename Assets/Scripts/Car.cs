using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    private float velocity;
    private float x;
    private float y;
    private float yaw;
    public float max_steering;
    public float L;
    public float max_velocity;

    // Start is called before the first frame update
    void Start()
    {
        velocity = 0f;
        x = 20.1f;
        y = 4.072672f;
        yaw = 0f;
        max_steering = 1f;
        L = 1f;
        max_velocity = 5f; // Imposta la velocità massima desiderata
    }

    
        // Update is called once per frame
        void Update()
        {
            if (Input.GetKey(KeyCode.UpArrow))
                Move(0, -1);
            if (Input.GetKey(KeyCode.LeftArrow))
                Move(-1, 0);
            if (Input.GetKey(KeyCode.RightArrow))
                Move(1, 0);

            transform.position = new Vector3(x, y, 56.3f);
            transform.rotation = Quaternion.Euler(0, yaw * Mathf.Rad2Deg, 0);
        }
    

    public void Move(float steering, float accel)
    {
        float delta = Mathf.Clamp(steering, -max_steering, max_steering);
        float dt = Time.deltaTime;

        // Calcola la velocità laterale e longitudinale
        float lateralVelocity = velocity * Mathf.Tan(delta) / L;
        float longitudinalVelocity = accel * max_velocity; // Moltiplica per la velocità massima desiderata

        // Calcola la nuova posizione e orientazione
        x += lateralVelocity * Mathf.Cos(yaw) * dt;
        y += lateralVelocity * Mathf.Sin(yaw) * dt;
        yaw += velocity * Mathf.Tan(delta) * dt;

        // Aggiorna la velocità
        velocity += longitudinalVelocity * dt;

        // Limita la yaw tra -PI e PI
        yaw = Mathf.Clamp(yaw, -Mathf.PI, Mathf.PI);
    }
}




/*public class Car : MonoBehaviour
{
    private float velocity;
    private float x;
    private float y;
    private float yaw;
    public float max_steering;
    public float L;
    // Start is called before the first frame update
    void Start()
    {
        velocity = 0f;
        x = 20.1f;
        y = 4.072672f;
        yaw = 0f;
        max_steering = 1f;
        L = 1f;
}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
            Move(0, -1);
        if (Input.GetKey(KeyCode.LeftArrow))
            Move(-1, 0);
        if (Input.GetKey(KeyCode.RightArrow))
            Move(1, 0);
        

        transform.position = new Vector3(x, y, 56.3f);
        transform.Rotate(Vector3.up, yaw);
    }

    public void Move(float steering, float accel)
    {
        float delta = Mathf.Clamp(steering, -max_steering, max_steering);
        float dt = Time.deltaTime;
        float dx = velocity * Mathf.Cos(yaw) * dt;
        float dy = velocity * Mathf.Sin(yaw) * dt;

        float dyaw = velocity / L * Mathf.Tan(delta) * dt;
        float dvelocity = accel * dt;

        x += dx;
        y += dy;
        yaw += dyaw;
        yaw = Mathf.Repeat(yaw + Mathf.PI, 2 * Mathf.PI) - Mathf.PI;
        velocity += dvelocity;





    }

}
*/    