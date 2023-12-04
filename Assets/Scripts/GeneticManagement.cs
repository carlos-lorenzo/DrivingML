using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using System.Linq;

public class GeneticManagement : MonoBehaviour
{
    // Hyperparameters
    public int movesSize = 10;
    public float acquiredMutationChance = 0.002f; // Lower chance for acquired moves
    public float explorationMutationChance = 0.01f; // Higher chance for new moves


    public int populationSize = 300;

    public int addMoveEveryNGenerations = 10;

    public GameObject agent;

    
    public int generation = 0;
    private bool completed = true;

    private Vector3 spawn = new Vector3(-205, 0, -0.1f);

    private List<GameObject> population;

    

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
                agentMoves.Mutate(acquiredMutationChance, explorationMutationChance);
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

        List<float> fitnessScores = new List<float>();

        foreach(GameObject agent in population) {
            float currentFitness = agent.GetComponent<AgentMoves>().fitness;
            fitnessScores.Add(currentFitness);
        }


        List<GameObject> fittestParents = new List<GameObject>(); 
        for (int i = 0; i < n; i++)
        {
            float maxValue = fitnessScores.Max();
            int maxIndex = fitnessScores.ToList().IndexOf(maxValue);
            fittestParents.Add(population[maxIndex]);
            fitnessScores.RemoveAt(maxIndex);
        }
        
        return fittestParents;

    }


    void Purge() {
        foreach (GameObject agent in population) {
            Destroy(agent);
        }

        population = new List<GameObject>();
    }


    
}
