using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AgentMoves : MonoBehaviour
{


    public List<int> horizontalMovements = new List<int>();
    public List<int> verticalMovements = new List<int>();

    private Drive driveScript;

    public float fitness;
    public bool collided = false;
    public bool finished = false;
    public int lastCheckpoint = -1;

    public int checkpointReward = 10;
    public int backwardsPunishment = 2000;

    public int collisionPunishment = 3;

    private System.Random random = new System.Random();

    private Rigidbody2D rb;

    private void Awake() {
        driveScript = gameObject.GetComponent<Drive>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }
  

    int RandomMove() {
        var choices = new List<int>{-1, 0, 1};
        return choices[random.Next(choices.Count)];
    }

    
    public void InitializeMoves(int movesSize) 
    {
        for (int i = 0; i < movesSize; i++)
        {
            horizontalMovements.Add(RandomMove());
            verticalMovements.Add(RandomMove());
        }
    }

    public void InheritMoves(int movesSize, List<List<int>> parent1Moves, List<List<int>> parent2Moves) 
    {
        
        
        int cutPoint = UnityEngine.Random.Range(0, parent1Moves[0].Count); // Select a random cut point for both parents

        List<int> newHorizontalMoves = new List<int>();
        List<int> newVerticalMoves = new List<int>();

        newHorizontalMoves.AddRange(parent1Moves[0].GetRange(0, cutPoint));
        newVerticalMoves.AddRange(parent1Moves[1].GetRange(0, cutPoint));

        newHorizontalMoves.AddRange(parent2Moves[0].GetRange(cutPoint, parent2Moves[0].Count - cutPoint));
        newVerticalMoves.AddRange(parent2Moves[1].GetRange(cutPoint, parent2Moves[1].Count - cutPoint));

        for (int i = 0; i < newHorizontalMoves.Count; i++)
        {
            horizontalMovements.Add(newHorizontalMoves[i]);
            verticalMovements.Add(newVerticalMoves[i]);
        }
        
        if (horizontalMovements.Count != movesSize) {
            horizontalMovements.Add(RandomMove());
            verticalMovements.Add(RandomMove());
        }
        
    }



    public void Mutate(float mutationChance) {
        for (int i = 0; i < horizontalMovements.Count; i++)
        {
            

            if (UnityEngine.Random.Range(0.0f, 1.0f) + (i / 100)< mutationChance) {
                horizontalMovements[i] = RandomMove();
            }

            if (UnityEngine.Random.Range(0.0f, 1.0f) + (i / 100) < mutationChance) {
                verticalMovements[i] = RandomMove();
            }
        }
    }

    public IEnumerator Drive() {
        for (int i = 0; i < horizontalMovements.Count; i++)
        {
            
            driveScript.h = horizontalMovements[i];
            driveScript.v = verticalMovements[i];
            //fitness += gameObject.transform.position.y - fitness;
            yield return new WaitForSeconds(0.5f);
            
            
        }

        finished = true;
        
        driveScript.h = 0;
        driveScript.v = 0; 

        GameObject nextCheckpoint = GameObject.Find((lastCheckpoint + 1).ToString());

        fitness -= Vector3.Distance(nextCheckpoint.transform.position, gameObject.transform.position);
        
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
       
       

        if (other.gameObject.CompareTag("Track")) {
            collided = true;
            fitness -= collisionPunishment * rb.velocity.magnitude;
            return;
        }
    } 
}
