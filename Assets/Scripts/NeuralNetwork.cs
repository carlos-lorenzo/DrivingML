using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor.UI;

public class NeuralNetwork : MonoBehaviour
{
    public List<int> size1 = new List<int> {2, 3};
    public List<int> size2 = new List<int> {3, 2};
    void Start() {
        Layer_Dense layer1 = new Layer_Dense(size1);
        Activation_ReLU activation1 = new Activation_ReLU();
        
        Layer_Dense layer2 = new Layer_Dense(size2);
        Activation_TanH activation2 = new Activation_TanH();

        layer1.Forward(new List<double> {1, 2});
        activation1.Forward(layer1.output);

        layer2.Forward(activation1.output);
        activation2.Forward(layer2.output);

        foreach (double output in activation2.output) {
            Debug.Log(output);
        }
    }
}

public class NumCs
{
    
    public List<double> Maximum(List<double> array1, List<double> array2) {
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
        
        List<double> result = new List<double>();
        
        for (int i = 0; i < array1.Count; i++)
        {

            result.Add(Math.Max(array1[i], array2[i]));
        }

        return result;
    }

    public List<List<double>> CreateRandom2DList(List<int> size) {
        if (size == null || size.Count != 2 || size[0] <= 0 || size[1] <= 0)
        {
            Debug.LogError("Invalid size provided!");
            return null; // Handle error or return null according to your needs
        }

        List<List<double>> result = new List<List<double>>();

        System.Random rand = new System.Random();
        for (int i = 0; i < size[0]; i++)
        {
            List<double> innerList = new List<double>();
            for (int j = 0; j < size[1]; j++)
            {
                innerList.Add((double)rand.NextDouble());
            }
            result.Add(innerList);
        }

        return result;
    }

    public List<List<double>> Num2D(List<int> size, double n) {

        if (size == null || size.Count != 2 || size[0] <= 0 || size[1] <= 0)
        {
            Debug.LogError("Invalid size provided!");
            return null; // Handle error or return null according to your needs
        }

        List<List<double>> result = new List<List<double>>();

        System.Random rand = new System.Random();
        for (int i = 0; i < size[0]; i++)
        {
            List<double> innerList = new List<double>();
            for (int j = 0; j < size[1]; j++)
            {
                innerList.Add(n);
            }
            result.Add(innerList);
        }

        return result;
    }

    public List<double> Num1D(int size, double n)
    {
        if ( size <= 0)
        {
            Debug.LogError("Invalid size provided!");
            return null; // Handle error or return null according to your needs
        }

        List<double> result = new List<double>();

        for (int i = 0; i < size; i++)
        {
            result.Add(n);
        }

        return result;
    }

    public List<double> DotProduct(List<double> vector, List<List<double>> matrix)
    {
        if (vector == null || matrix == null || vector.Count != matrix.Count || matrix.Count == 0)
        {
            Debug.LogError("Invalid input arrays!");
            return null; // Return null or handle error according to your needs
        }

        int rows = matrix.Count;
        int cols = matrix[0].Count;

        List<double> result = new List<double>();

        for (int i = 0; i < cols; i++)
        {
            double sum = 0;
            for (int j = 0; j < rows; j++)
            {
                sum += matrix[j][i] * vector[j];
            }
            result.Add(sum);
        }

        return result;
    }

    public List<double> VectorisedSum(List<double> vector1, List<double> vector2)
    {
        
        if (vector1 == null || vector2 == null || vector1.Count != vector2.Count)
        {
            Debug.LogError("Invalid input vectors!");
            return null; // Return null or handle error according to your needs
        }

        List<double> result = new List<double>();

        for (int i = 0; i < vector1.Count; i++)
        {
            result.Add(vector1[i] + vector2[i]);
        }

        return result;
    }

    public List<double> VectorisedDivision(List<double> vector1, List<double> vector2)
    {
        
        if (vector1 == null || vector2 == null || vector1.Count != vector2.Count)
        {
            Debug.LogError("Invalid input vectors!");
            return null; // Return null or handle error according to your needs
        }

        List<double> result = new List<double>();

        for (int i = 0; i < vector1.Count; i++)
        {
            result.Add(vector1[i] / vector2[i]);
        }

        return result;
    }

    public List<double> VectorisedMultiplication(List<double> vector1, List<double> vector2)
    {
        
        if (vector1 == null || vector2 == null || vector1.Count != vector2.Count)
        {
            Debug.LogError("Invalid input vectors!");
            return null; // Return null or handle error according to your needs
        }

        List<double> result = new List<double>();

        for (int i = 0; i < vector1.Count; i++)
        {
            result.Add(vector1[i] * vector2[i]);
        }

        return result;
    }

    public List<double> ScalarMultiplication(double n, List<double> vector)
    {
        
        if ( vector == null)
        {
            Debug.LogError("Invalid input vector");
            return null; // Return null or handle error according to your needs
        }

        List<double> result = new List<double>();

        for (int i = 0; i < vector.Count; i++)
        {
            result.Add(n * vector[i]);
        }

        return result;
    }

    public List<double> ScalarDivision(double n, List<double> vector)
    {
        
        if ( vector == null)
        {
            Debug.LogError("Invalid input vector");
            return null; // Return null or handle error according to your needs
        }

        List<double> result = new List<double>();

        for (int i = 0; i < vector.Count; i++)
        {
            result.Add(n / vector[i]);
        }

        return result;
    }

    public List<double> ScalarAddition(double n, List<double> vector)
    {
        
        if ( vector == null)
        {
            Debug.LogError("Invalid input vector");
            return null; // Return null or handle error according to your needs
        }

        List<double> result = new List<double>();

        for (int i = 0; i < vector.Count; i++)
        {
            result.Add(n + vector[i]);
        }

        return result;
    }

    public List<double> ScalarSubtraction(double n, List<double> vector)
    {
        
        if ( vector == null)
        {
            Debug.LogError("Invalid input vector");
            return null; // Return null or handle error according to your needs
        }

        List<double> result = new List<double>();

        for (int i = 0; i < vector.Count; i++)
        {
            result.Add(n - vector[i]);
        }

        return result;
    }

    public List<double> ScalarPower(double powerBase, List<double> exponentVector)
    {
        
        if (exponentVector == null )
        {
            Debug.LogError("Invalid input vector!");
            return null; // Return null or handle error according to your needs
        }

        List<double> result = new List<double>();

        for (int i = 0; i < exponentVector.Count; i++)
        {
            result.Add(Math.Pow(powerBase, exponentVector[i]));
        }

        return result;
    }

}

interface IActivationFuntion {
    
    public void Forward(List<double> inputs);

    public void Backward(List<double> dvalues);
}

class Activation_ReLU : IActivationFuntion {

    public List<double> inputs;
    public List<double> output;
    public List<double> dinputs;

    private readonly NumCs nc = new NumCs();

    public void Forward(List<double> inputs) {
        this.inputs = inputs;
        output = nc.Maximum(nc.Num1D(inputs.Count, 0), inputs);
    }

    public void Backward(List<double> dvalues) {
        dinputs = nc.Maximum(null, dinputs);
    }
}

class Activation_TanH : IActivationFuntion {
    public List<double> inputs;
    public List<double> output;
    public List<double> dinputs;

    private readonly NumCs nc = new NumCs();

    public void Forward(List<double> inputs) {
        output = nc.ScalarSubtraction(1, nc.ScalarDivision(2, nc.ScalarAddition(1, nc.ScalarPower(Math.E, nc.ScalarMultiplication(-2, inputs)))));
    }

    public void Backward(List<double> dvalues) {
        
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

    public List<double> inputs;
    public List<double> output;

    public List<List<double>> weights;
    public List<double> biases;

    public readonly List<int> size;

    private readonly NumCs nc = new NumCs();

    public Layer_Dense(List<int> size) {
        this.size = size;
        weights = nc.CreateRandom2DList(size);

        biases = nc.Num1D(this.size[1], 0);
    }

    public void Forward(List<double> inputs) {
        this.inputs = inputs;
        output = nc.VectorisedSum(nc.DotProduct(inputs, weights), biases);
    }

    public void DebugWeights() {
        Debug.Log("Weights: ");
        foreach(List<double> weightI in weights) {
            foreach(double weight in weightI) {
                Debug.Log(weight);
         
            }
        }

        Debug.Log("----------");
    }

    public void DebugBiases() {
        Debug.Log("Biases: ");
        foreach(double bias in biases) {
            
            Debug.Log(bias);
        
        }

        Debug.Log("----------");
    }
}