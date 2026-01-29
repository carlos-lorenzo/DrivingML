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

    public int checkpointReward = 100;
    public int backwardsPunishment = 500;

    public int collisionPunishment = 1;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    void FixedUpdate()
    {
        
        //Vector2 speed = transform.up * (v * acceleration);

        // Calculate the desired velocity
        float targetVelocity = v * acceleration; // * speed

        // Smoothly adjust the current velocity towards the target velocity
        currentVelocity = Mathf.Lerp(currentVelocity, targetVelocity, Time.deltaTime * 5f);

        // Apply the smoothed velocity as force
        rb.AddForce(transform.up * currentVelocity);

        float direction = Vector2.Dot(rb.linearVelocity, rb.GetRelativeVector(Vector2.up));
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

        float driftForce = Vector2.Dot(rb.linearVelocity, rb.GetRelativeVector(rightAngleFromForward.normalized));
        Vector2 relativeForce = rightAngleFromForward.normalized * -1.0f * (driftForce * 10.0f);

        rb.AddForce(rb.GetRelativeVector(relativeForce));
        fitness -= 0.0001f;
        if (finished) {
            v = 0;
            h = 0;
            rb.linearVelocity = Vector2.zero;
            UpdateFitness();
        }
    }

    

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Checkpoint")) {
            int checkpointNumber = int.Parse(other.gameObject.name);

            if (checkpointNumber == lastCheckpoint + 1) {
                lastCheckpoint = checkpointNumber;
                fitness += checkpointReward;
            } else {
                //finished = true;
                fitness -= backwardsPunishment;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
       
        //if (collided) {return;}

        if (other.gameObject.CompareTag("Track")) {
            //GameObject.Find("NeuralNetwork").GetComponent<DeepGeneticManagement>().collided++;

            collided = true;
            //finished = true;
            fitness -= collisionPunishment;
        }
    } 
    
    public void UpdateFitness() {
        
        GameObject nextCheckpoint = GameObject.Find((lastCheckpoint + 1).ToString());

        fitness -= Vector3.Distance(nextCheckpoint.transform.position, gameObject.transform.position);

        
    }
}
