using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkTest : MonoBehaviour
{
  


    private NumCs nc = new NumCs();
    Layer_Dense layer1 = new Layer_Dense(2, 32);
    Activation_ReLU activation1 = new Activation_ReLU();

    
    Layer_Dense layer2 = new Layer_Dense(32, 3);

    Activation_Softmax_Loss_CategoricalCrossEntropy activation_loss = new Activation_Softmax_Loss_CategoricalCrossEntropy();

    Optimiser_SGD optimiser = new Optimiser_SGD(0.001);
    
    public int epochs = 600;

    public List<List<double>> ForwardPass(List<List<double>> X, List<List<double>> ytrue) {
        
        // Forward pass
        layer1.Forward(X);
        activation1.Forward(layer1.output);


        layer2.Forward(activation1.output);
        

        activation_loss.Forward(layer2.output, ytrue);
       
        return activation_loss.loss.Calculate(activation_loss.output, ytrue);
    }

    public void Backpropagation(List<List<double>> ytrue) {
        

        activation_loss.Backward(activation_loss.output, ytrue);
       
        //activation2.Backward(lossfn.dinputs);
        
        layer2.Backward(activation_loss.dinputs);
        
        activation1.Backward(layer2.dinputs);
        layer1.Backward(activation1.dinputs);

        
        optimiser.UpdateParams(layer1);
        
        optimiser.UpdateParams(layer2);
    }

    public void Train(List<List<double>> X, List<List<double>> y) {
        for (int epoch = 0; epoch < epochs; epoch++)
        {
            var loss = ForwardPass(X, y);
            Backpropagation(y);
        
            if (epoch % 100 == 0) {
                string trainingState = String.Format("Acc: {0} | Loss: {1}", Math.Round(Accuracy(activation_loss.output, y), 2), Math.Round(loss[0][0], 2));
                Debug.Log(trainingState);
            }
        }
    }

     public List<List<List<double>>> GenerateSpiralDataset(int samples, int classes)
    {
        List<List<List<double>>> dataset = new List<List<List<double>>>();
        System.Random rand = new System.Random();

        List<List<double>> points = new List<List<double>>();
        List<List<double>> oneHotLabels = new List<List<double>>();

        for (int i = 0; i < samples * classes; i++)
        {
            double radius = (double)i / samples / classes * 2 * Math.PI;
            double noise = rand.NextDouble() * 0.01;
            double t = (double)(i % classes) / classes * 2 * Math.PI + radius + noise;
            double x = radius * Math.Sin(t);
            double y = radius * Math.Cos(t);

            points.Add(new List<double> { x, y });

            List<double> label = new List<double>();
            for (int k = 0; k < classes; k++)
            {
                label.Add(k == i % classes ? 1.0 : 0.0);
            }
            oneHotLabels.Add(label);
        }

        dataset.Add(points);
        dataset.Add(oneHotLabels);

        return dataset;
    }

    public double Accuracy(List<List<double>> ypred, List<List<double>> ytrue) {
        
        
        List<List<double>> trueIndices = nc.ArgMax(ytrue);
        List<List<double>> predictions = nc.ArgMax(ypred);

        double correntPredictions = 0;

        for (int i = 0; i < ypred.Count; i++)
        {
            if (trueIndices[i][0] == predictions[i][0]) {
                correntPredictions++;
            } 
        }
        

        return correntPredictions / ypred.Count;
    }

    void Start() {

        int numSamples = 250;
        int numClasses = 3;

        List<List<List<double>>> dataset = GenerateSpiralDataset(numSamples, numClasses);
        List<List<double>> X = dataset[0];
        List<List<double>> y = dataset[1];

        
        X = nc.ScalarMultiplication(0.1, X);
        
        Train(X, y);
        
    }

        
}


