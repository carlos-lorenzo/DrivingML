using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualDrive : MonoBehaviour
{

    public float acceleration;
    public float steering;

    private Rigidbody2D rb;

    public float h;
    public float v;

    public float currentVelocity; 


    float maxDistance = 250;

    public float distanceToCollider;

    public LayerMask layerMask;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        //Debug.DrawRay(transform.position, transform.up * 100, Color.red);

    }

    void FixedUpdate()
    {
        v = Input.GetAxis("Vertical");

        h = -Input.GetAxis("Horizontal");

         // Cast a ray in the forward direction of the object
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, maxDistance);
        if (hit.collider != null)
        {
            // Check if the ray hits a collider
            Debug.DrawRay(transform.position, transform.up * hit.distance, Color.green);
            
            distanceToCollider = hit.distance;
            // Use distanceToCollider in your logic here
        } else {
            // Ray doesn't hit anything
            Debug.DrawRay(transform.position, transform.up * maxDistance, Color.red);
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




}
