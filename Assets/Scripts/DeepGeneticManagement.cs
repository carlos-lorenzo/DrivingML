using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using System.Linq;

public class DeepGeneticManagement : MonoBehaviour
{
    
    
    public double deltaMagnitude = 0.1;

    public int populationSize = 300;


    public GameObject agent;

    
    public int generation = -1;
    public int simLength = 2;
    public int increaseLengthEveryNGenerations = 5;
    
    private bool completed = true;

    private Vector3 spawn = new Vector3(-205, 0, -0.1f);

    private List<GameObject> population = new List<GameObject>();

    private List<List<List<double>>> parentWeights = new List<List<List<double>>>();
    private List<List<List<double>>> parentBiases = new List<List<List<double>>>();
    
    void Start() {
        Populate();
    }

    
    void FixedUpdate() {
        
        if (generation > 0) {
            DeepDrive agentMoves = population[^1].GetComponent<DeepDrive>();
            if (agentMoves != null) {
                if (agentMoves.finished) {
                    
                    completed = true;
                }
            }
            
        }
        

        if (completed) {
            Populate();
            generation += 1;
            completed = false;

            if (generation % increaseLengthEveryNGenerations == 0) {
                simLength += 1;
            }
            
        }
        
    }


    void Populate() {
        
        
        
        if (generation > 0){
            GameObject fittestParent = FittestParents(1)[0];
            GeneticNeuralNetwork parentNN = fittestParent.GetComponent<GeneticNeuralNetwork>();
            

            parentWeights.Add(parentNN.layer1.weights);
            parentWeights.Add(parentNN.layer2.weights);

            parentBiases.Add(parentNN.layer1.biases);
            parentBiases.Add(parentNN.layer2.biases);
            Purge();
            
            

        }

        for (int i = 0; i < populationSize; i++)
        {
            GameObject newAgent = Instantiate(agent, spawn, quaternion.identity);
            newAgent.GetComponent<GeneticNeuralNetwork>().simLength = simLength;
            population.Add(newAgent);

        }

        if (generation > 0) {
            foreach(GameObject agent in population) {
                GeneticNeuralNetwork agentNN = agent.GetComponent<GeneticNeuralNetwork>();
                agentNN.InheritParameters(parentWeights, parentBiases);
                agentNN.TuneParameters(deltaMagnitude);
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
