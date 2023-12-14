using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeepDrive : MonoBehaviour
{
    public bool collided;

    public float acceleration;
    public float steering;

    private Rigidbody2D rb;

    public float h;
    public float v;

    public float currentVelocity; 

    public float fitness;
    public bool finished = false;
    public int lastCheckpoint = -1;

    public int checkpointReward = 10;
    public int backwardsPunishment = 2000;

    public int collisionPunishment = 3;

    float maxDistance = 20f;

    public List<List<double>> inputs;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(UpdaFitness());
    }

    void FixedUpdate()
    {
        

         // Cast a ray in the forward direction of the object
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance))
        {
            // Check if the ray hits a collider
            Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.green);
            
            float distanceToCollider = hit.distance;
            // Use distanceToCollider in your logic here
        }
        else
        {
            // Ray doesn't hit anything
            Debug.DrawRay(transform.position, transform.forward * maxDistance, Color.red);
        }

        Vector2 speed = transform.up * (v * acceleration);

        // Calculate the desired velocity
        float targetVelocity = v * acceleration; // * speed

        // Smoothly adjust the current velocity towards the target velocity
        currentVelocity = Mathf.Lerp(currentVelocity, targetVelocity, Time.deltaTime * 5f);

        // Apply the smoothed velocity as force
        rb.AddForce(transform.up * currentVelocity);

        float direction = Vector2.Dot(rb.velocity, rb.GetRelativeVector(Vector2.up));
        if (direction >= 0.0f)
        {
            rb.rotation += h * steering;
        }
        else
        {
            rb.rotation -= h * steering;
        }

        Vector2 forward = new Vector2(0.0f, 0.5f);
        float steeringRightAngle;
        if (rb.angularVelocity > 0)
        {
            steeringRightAngle = -90;
        }
        else
        {
            steeringRightAngle = 90;
        }

        Vector2 rightAngleFromForward = Quaternion.AngleAxis(steeringRightAngle, Vector3.forward) * forward;

        float driftForce = Vector2.Dot(rb.velocity, rb.GetRelativeVector(rightAngleFromForward.normalized));
        Vector2 relativeForce = (rightAngleFromForward.normalized * -1.0f) * (driftForce * 10.0f);

        rb.AddForce(rb.GetRelativeVector(relativeForce));
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Checkpoint")) {
            int checkpointNumber = int.Parse(other.gameObject.name);

            if (checkpointNumber == lastCheckpoint + 1) {
                lastCheckpoint = checkpointNumber;
                fitness += checkpointReward;
            } else {
                fitness -= backwardsPunishment;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
       
        if (collided) {return;}

        if (other.gameObject.CompareTag("Track")) {
            collided = true;
            fitness -= collisionPunishment;
            return;
        }
    } 
    
    public IEnumerator UpdaFitness() {
        
        GameObject nextCheckpoint = GameObject.Find((lastCheckpoint + 1).ToString());

        fitness -= Vector3.Distance(nextCheckpoint.transform.position, gameObject.transform.position);

        yield return new WaitForSeconds(0.5f);
        
    }
}
