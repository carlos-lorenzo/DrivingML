using UnityEngine;
using System.Collections.Generic;

public class AgentView : MonoBehaviour
{
    [SerializeField]
    private LayerMask layers;

    [SerializeField]
    private float maxDistance = 10f;

    [SerializeField]
    private List<float> directions = new List<float>  { -75f, -35f, 0f, 35f, 75f };
    public List<List<double>> inputs; // Array to store distances
    
    private Rigidbody2D rb;

    void Start() {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        inputs = new List<List<double>>
        {
            new List<double>()
        };
       
        for (int i = 0; i < directions.Count; i++)
        {
            float angle = directions[i];
            Vector2 rayDirection = Quaternion.Euler(0, 0, angle) * transform.up;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, maxDistance, layers);

            if (hit.collider != null)
            {
                inputs[0].Add((double)hit.distance);
                //Debug.DrawRay(transform.position, rayDirection * inputs[0][i], Color.green);
            }
            else
            {
                inputs[0].Add(-1); // Set distance to -1 if ray doesn't hit anything
                //Debug.DrawRay(transform.position, rayDirection * maxDistance, Color.red);
            }
        }

        float velocity = rb.linearVelocity.magnitude;

        if (velocity <= 1e-5){
            velocity = 0;
        }
        
        inputs[0].Add(velocity);
        inputs[0].Add(Vector3.SignedAngle(rb.rotation * Vector3.up, rb.linearVelocity, Vector3.up));
    }
}
