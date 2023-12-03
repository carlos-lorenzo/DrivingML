using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class GeneticManagement : MonoBehaviour
{
    // Hyperparameters
    public int movesSize = 10;
    public float mutationChance = 0.005f;

    public int populationSize = 300;

    public int addMoveEveryNGenerations = 10;

    public GameObject agent;

    
    public int generation = 0;
    private bool completed = true;

    private Vector3 spawn = new Vector3(-205, 0, -0.1f);

    private List<GameObject> population;

    

    private System.Random random = new System.Random();
    

    void FixedUpdate() {
        
        if (generation != 0) {
            AgentMoves agentMoves = population[population.Count - 1].GetComponent<AgentMoves>();
            if (agentMoves != null) {
                if (agentMoves.finished) {
                    completed = true;
                }
            }
            
        }
        

        if (completed) {
            Populate();
            Live();
            generation += 1;
            completed = false;

            if (generation % addMoveEveryNGenerations == 0) {
                movesSize += 1;
            }
            
        }
        
    }



    void Populate() {
        List<List<int>> parent1Moves = new List<List<int>>();
        List<List<int>> parent2Moves = new List<List<int>>();

        if (generation != 0)
        {
            List<GameObject> parents = FittestParents(2);

            parent1Moves.Add(parents[0].GetComponent<AgentMoves>().horizontalMovements);
            parent1Moves.Add(parents[0].GetComponent<AgentMoves>().verticalMovements);

            parent2Moves.Add(parents[1].GetComponent<AgentMoves>().horizontalMovements);
            parent2Moves.Add(parents[1].GetComponent<AgentMoves>().verticalMovements);

            Purge();

        }

        population = new List<GameObject>();

        for (int i = 0; i < populationSize; i++)
        {
            GameObject newAgent = Instantiate(agent, spawn, quaternion.identity);
            AgentMoves agentMoves = newAgent.GetComponent<AgentMoves>();

            if (agentMoves == null)
            {
                continue;
            }

            if (generation != 0)
            {
                agentMoves.InheritMoves(movesSize, parent1Moves, parent2Moves);
                agentMoves.Mutate(mutationChance);
            }
            else
            {
                agentMoves.InitializeMoves(movesSize);
            }

            population.Add(newAgent);
        }
    }   


    void Live() {
        foreach (GameObject agent in population) {
            if (agent.TryGetComponent<AgentMoves>(out var agentMoves)) {
                StartCoroutine(agentMoves.Drive());
            }
            
        }
    }


    List<GameObject> FittestParents(int n) {
        // Sort the population by fitness
        population.Sort((x, y) => y.GetComponent<AgentMoves>().fitness.CompareTo(x.GetComponent<AgentMoves>().fitness));

        // Get the best n agents
        List<GameObject> fittestParents = population.GetRange(0, Mathf.Min(n, population.Count));

        return fittestParents;
    }


    void Purge() {
        foreach (GameObject agent in population) {
            Destroy(agent);
        }

        population = new List<GameObject>();
    }

    int RandomMove() {
        var choices = new List<int>{-1, 0, 1};
        return choices[random.Next(choices.Count)];
    }

    List<int> Randomise(List<int> moves) {
        for (int i = 0; i < moves.Count; i++)
        {
            if (UnityEngine.Random.Range(0.0f, 1.0f) < mutationChance) {
                moves[i] = RandomMove();
            }
        }

        return moves;
    }

    
}
