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

    public float distanceToCollider;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        v = Input.GetAxis("Vertical");

        h = -Input.GetAxis("Horizontal");


        Vector2 speed = transform.up * (v * acceleration);

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
        Vector2 relativeForce = (rightAngleFromForward.normalized * -1.0f) * (driftForce * 10.0f);

        rb.AddForce(rb.GetRelativeVector(relativeForce));
    }




}
