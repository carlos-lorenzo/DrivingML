using UnityEngine;
using System;
using System.Collections.Generic;

public class NeuralNetwork : MonoBehaviour
{
    public List<int> size1 = new List<int> {2, 3};
    public List<int> size2 = new List<int> {3, 2};
    void Start() {
        Layer_Dense layer1 = new Layer_Dense(size1);
        Activation_ReLU activation1 = new Activation_ReLU();
        
        Layer_Dense layer2 = new Layer_Dense(size2);
        Activation_ReLU activation2 = new Activation_ReLU();

        layer1.Forward(new List<float> {1, 2});
        activation1.Forward(layer1.output);

        layer2.Forward(activation1.output);
        activation2.Forward(layer2.output);

        foreach (float output in activation2.output) {
            Debug.Log(output);
        }
    }
}

public class NumCs
{
    
    public List<float> Maximum(List<float> array1, List<float> array2) {
        if (array1 == null || array2 == null)
        {
            Debug.LogError("Both arrays cannot be null!");
            return null; // Return null or handle error according to your needs
        }

        if (array1.Count != array2.Count)
        {
            Debug.LogError("Both arrays must have the same size");
            return null; // Return null or handle error according to your needs
        }
        
        List<float> result = new List<float>();
        
        for (int i = 0; i < array1.Count; i++)
        {

            result.Add(Math.Max(array1[i], array2[i]));
        }

        return result;
    }

    public List<List<float>> CreateRandom2DList(List<int> size) {
        if (size == null || size.Count != 2 || size[0] <= 0 || size[1] <= 0)
        {
            Debug.LogError("Invalid size provided!");
            return null; // Handle error or return null according to your needs
        }

        List<List<float>> result = new List<List<float>>();

        System.Random rand = new System.Random();
        for (int i = 0; i < size[0]; i++)
        {
            List<float> innerList = new List<float>();
            for (int j = 0; j < size[1]; j++)
            {
                innerList.Add((float)rand.NextDouble());
            }
            result.Add(innerList);
        }

        return result;
    }

    public List<List<float>> Zeros2D(List<int> size) {

        if (size == null || size.Count != 2 || size[0] <= 0 || size[1] <= 0)
        {
            Debug.LogError("Invalid size provided!");
            return null; // Handle error or return null according to your needs
        }

        List<List<float>> result = new List<List<float>>();

        System.Random rand = new System.Random();
        for (int i = 0; i < size[0]; i++)
        {
            List<float> innerList = new List<float>();
            for (int j = 0; j < size[1]; j++)
            {
                innerList.Add(0);
            }
            result.Add(innerList);
        }

        return result;
    }

    public List<float> Zeros1D(List<int> size)
    {
        if (size == null || size.Count != 1 || size[0] <= 0)
        {
            Debug.LogError("Invalid size provided!");
            return null; // Handle error or return null according to your needs
        }

        List<float> result = new List<float>();

        for (int i = 0; i < size[0]; i++)
        {
            result.Add(0);
        }

        return result;
    }

    public List<float> DotProduct(List<float> vector, List<List<float>> matrix)
    {
        if (vector == null || matrix == null || vector.Count != matrix.Count || matrix.Count == 0)
        {
            Debug.LogError("Invalid input arrays!");
            return null; // Return null or handle error according to your needs
        }

        int rows = matrix.Count;
        int cols = matrix[0].Count;

        List<float> result = new List<float>();

        for (int i = 0; i < cols; i++)
        {
            float sum = 0;
            for (int j = 0; j < rows; j++)
            {
                sum += matrix[j][i] * vector[j];
            }
            result.Add(sum);
        }

        return result;
    }

    public List<float> VectorisedSum(List<float> vector1, List<float> vector2)
    {
        
        if (vector1 == null || vector2 == null || vector1.Count != vector2.Count)
        {
            Debug.LogError("Invalid input vectors!");
            return null; // Return null or handle error according to your needs
        }

        List<float> result = new List<float>();

        for (int i = 0; i < vector1.Count; i++)
        {
            result.Add(vector1[i] + vector2[i]);
        }

        return result;
    }


}

interface IActivationFuntion {
    
    public void Forward(List<float> inputs);

    public void Backward(List<float> dvalues);
}

class Activation_ReLU : IActivationFuntion {

    public List<float> inputs;
    public List<float> output;
    public List<float> dinputs;

    private readonly NumCs nc = new NumCs();

    public void Forward(List<float> inputs) {
        this.inputs = inputs;
        output = nc.Maximum(nc.Zeros1D(new List<int> {inputs.Count}), inputs);
    }

    public void Backward(List<float> dvalues) {
        dinputs = nc.Maximum(null, dinputs);
    }
}


interface ILoss {
    public void Forward();
    public void Backward();
}

class Loss_PolicyLoss : ILoss {
    public void Forward() {

    }

    public void Backward() {
        
    }
}

class Layer_Dense {

    public List<float> inputs;
    public List<float> output;

    public List<List<float>> weights;
    public List<float> biases;

    public readonly List<int> size;

    private readonly NumCs nc = new NumCs();

    public Layer_Dense(List<int> size) {
        this.size = size;
        weights = nc.CreateRandom2DList(size);

        List<int> sizeBiases = new List<int> {this.size[1]};
        biases = nc.Zeros1D(sizeBiases);
    }

    public void Forward(List<float> inputs) {
        this.inputs = inputs;
        output = nc.VectorisedSum(nc.DotProduct(inputs, weights), biases);
    }

    public void DebugWeights() {
        Debug.Log("Weights: ");
        foreach(List<float> weightI in weights) {
            foreach(float weight in weightI) {
                Debug.Log(weight);
         
            }
        }

        Debug.Log("----------");
    }

    public void DebugBiases() {
        Debug.Log("Biases: ");
        foreach(float bias in biases) {
            
            Debug.Log(bias);
        
        }

        Debug.Log("----------");
    }
}