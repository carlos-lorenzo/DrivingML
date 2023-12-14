using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentView : MonoBehaviour
{
    
    Vector3 debugDirection = Vector3.up;
    Vector2 rayDirection = Vector2.up;
    public LayerMask layers;
    public float distance;
    // Update is called once per frame
    void FixedUpdate()
    {
        var heading = Math.Atan2(transform.right.z, transform.right.x) * Mathf.Rad2Deg;
        RaycastHit2D hit = Physics2D.Raycast(transform.up, rayDirection, Mathf.Infinity, layers);

        // If it hits something...
        if (hit.collider != null)
        {
            // Calculate the distance from the surface and the "error" relative
            // to the floating height.
            
            distance = hit.distance;
            
            Debug.DrawRay(transform.position, debugDirection * distance, Color.green);
            
        }
    }
}
