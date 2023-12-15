using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GeneticNeuralNetwork : MonoBehaviour
{
  
   

    private NumCs nc = new NumCs();
    public Layer_Dense layer1 = new Layer_Dense(7, 32);
    Activation_ReLU activation1 = new Activation_ReLU();

    
    public Layer_Dense layer2 = new Layer_Dense(32, 2);

    Activation_TanH activation2 = new Activation_TanH();
    
    DeepDrive deepDrive;
    AgentView agentView;
    public int moveChanges = 0;
    public int simLength;


    private void Start() {
        deepDrive = gameObject.GetComponent<DeepDrive>();
        agentView = gameObject.GetComponent<AgentView>();

        StartCoroutine(Drive());
    }

    
    private void FixedUpdate() {

        if (moveChanges == simLength) {
            StopCoroutine(Drive());
            deepDrive.finished = true;
            
        }
        
    }

    IEnumerator Drive() {
        moveChanges = 0;

        while (moveChanges < simLength){
            yield return new WaitForSeconds(0.1f);
            var directionChange = ForwardPass(agentView.inputs);

            deepDrive.h = (float)directionChange[0][0];
            deepDrive.v = (float)directionChange[0][1];
            moveChanges += 1;
        
        }
        
    }

    public List<List<double>> ForwardPass(List<List<double>> X) {
        
        // Forward pass
        layer1.Forward(X);
        activation1.Forward(layer1.output);


        layer2.Forward(activation1.output);
        

        activation2.Forward(layer2.output);
        

        return activation2.output;
        
    }


    public void TuneParameters(double deltaMagnitude) {
        
        
        // "Tune... more like praying"
        var weights1Shape = nc.ArrayShape(layer1.weights);
        layer1.weights = nc.ElementwiseAddition(layer1.weights, nc.ScalarMultiplication(deltaMagnitude, nc.Rand2D(weights1Shape[0], weights1Shape[1])));
        
        
        var biases1Shape = nc.ArrayShape(layer1.biases);
        layer1.biases = nc.ElementwiseAddition(layer1.biases, nc.ScalarMultiplication(deltaMagnitude, nc.Rand2D(biases1Shape[0], biases1Shape[1])));
        


        
        var weights2Shape = nc.ArrayShape(layer2.weights);
        layer2.weights = nc.ElementwiseAddition(layer2.weights, nc.ScalarMultiplication(deltaMagnitude, nc.Rand2D(weights2Shape[0], weights2Shape[1])));
        
        
        
        var biases2Shape = nc.ArrayShape(layer2.biases);
        layer2.biases = nc.ElementwiseAddition(layer2.biases, nc.ScalarMultiplication(deltaMagnitude, nc.Rand2D(biases2Shape[0], biases2Shape[1])));
        

    }


    public void InheritParameters(List<List<List<double>>> weights, List<List<List<double>>> biases) {
        layer1.weights = weights[0];
        layer1.biases = biases[0];

        layer2.weights = weights[1];
        layer2.biases = biases[1];
    } 
}


