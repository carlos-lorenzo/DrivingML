using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEditor.VersionControl;


public class NeuralNetwork : MonoBehaviour
{


    private NumCs nc = new NumCs();
    Layer_Dense layer1 = new Layer_Dense(2, 32);
    Activation_ReLU activation1 = new Activation_ReLU();

    
    Layer_Dense layer2 = new Layer_Dense(32, 2);
    Activation_ReLU activation2 = new Activation_ReLU();

    Activation_Softmax_Loss_CategoricalCrossEntropy activation_loss = new Activation_Softmax_Loss_CategoricalCrossEntropy();

    Optimiser_SGD optimiser = new Optimiser_SGD(0.001);
    
    public int epochs = 100;

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

        int numSamples = 100;
        int numClasses = 2;

        List<List<List<double>>> dataset = GenerateSpiralDataset(numSamples, numClasses);
        List<List<double>> X = dataset[0];
        List<List<double>> y = dataset[1];

        
        X = nc.ScalarMultiplication(0.1, X);
        
        Train(X, y);
        
    }

        
}

public class NumCs
{
    
    public List<List<double>> Maximum(double n, List<List<double>> arr) {

        /**  
            Modifies each element in a 2D array to be the maximum of the element and n.

            Args:
                n: The value to compare with each element in the array
                arr: The 2D array to be modified

            Returns:
                A new 2D array with modified values where each element is the maximum of the element and n
        **/

        if ( arr == null)
        {
            Debug.Log("Maximum - Array cannot be null!");
            return null; // Return null or handle error according to your needs
        }

        List<List<double>> result = new List<List<double>>();
        
        
        for (int i = 0; i < arr.Count; i++)
        {
            result.Add(new List<double>());
            for (int j = 0; j < arr[i].Count; j++)
            {
                result[i].Add(Math.Max(0, arr[i][j]));
            }
            
        }

        return result;
    }
   
    public List<List<double>> Rand2D(int dim1, int dim2) {
        /**  
            Returns a 2D array with given size, filled with random values
        **/

        if (dim1 <= 0 || dim2 <= 0)
        {
            Debug.Log(" Rand2D - Invalid size provided!");
            return null; // Handle error or return null according to your needs
        }

        List<List<double>> result = new List<List<double>>();

        System.Random rand = new System.Random();
        for (int i = 0; i < dim1; i++)
        {
            result.Add(new List<double>());
            for (int j = 0; j < dim2; j++)
            {
                result[i].Add((double)rand.NextDouble());
            }
            
        }

        return result;
    }

    public List<List<double>> Full(double n, int dim1, int dim2) {

        /**  
            Returns a 2D array with given size, filled with n
        **/

        if (dim1 <= 0 || dim2 <= 0)
        {
            Debug.Log("Invalid size provided!");
            return null; // Handle error or return null according to your needs
        }

        List<List<double>> result = new List<List<double>>();

        
        for (int i = 0; i < dim1; i++)
        {
            result.Add(new List<double> ());
            for (int j = 0; j < dim2; j++)
            {
                result[i].Add(n);
            }
            
        }

        return result;
    }


   public List<List<double>> MatMul(List<List<double>> matrix1, List<List<double>> matrix2)
    {

        /** 
            Returns the result between the product of 2 matricies

            Args:
                matrix1: First matrix
                matrix2: Second matrix

            Returns:
                Result between the product of 2 matricies
        
        **/
        if (matrix1 == null || matrix2 == null)
        {
            Debug.Log("Matmul - Matrices cannot be null");
            return null; // Handle error or return null according to your needs
        }

        int rowsA = matrix1.Count;
        int colsA = matrix1[0].Count;
        int rowsB = matrix2.Count;
        int colsB = matrix2[0].Count;

        if (colsA != rowsB)
        {
            Debug.Log("Matmul - Cannot multiply matrices: Incompatible dimensions");
            return null; // Handle error or return null according to your needs
        }

        List<List<double>> result = new List<List<double>>();

        for (int i = 0; i < rowsA; i++)
        {
            result.Add(new List<double>());
            for (int j = 0; j < colsB; j++)
            {
                double sum = 0;
                for (int k = 0; k < colsA; k++)
                {
                    sum += matrix1[i][k] * matrix2[k][j];
                }
                result[i].Add(sum);
            }
        }

        return result;
    }


    public List<List<double>> ElementwiseAddition(List<List<double>> array1, List<List<double>> array2)
    {
        if (array1 == null || array2 == null || array1.Count == 0 || array1.Count != array2.Count)
        {
            Debug.Log("EWA - Invalid input arrays!");
            return null;
        }

        int rows1 = array1.Count;
        int cols1 = array1[0].Count;
        int cols2 = array2[0].Count;

        List<List<double>> result = new List<List<double>>();

        for (int i = 0; i < rows1; i++)
        {
            List<double> newRow = new List<double>();
            for (int j = 0; j < cols1; j++)
            {
                double val1 = array1[i][j];
                double val2 = array2[i][j % cols2]; // Accessing corresponding element in array2

                newRow.Add(val1 + val2);
            }
            result.Add(newRow);
        }

        return result;
    }

    public List<List<double>> ElementwiseSubtraction(List<List<double>> array1, List<List<double>> array2)
    {
        if (array1 == null || array2 == null || array1.Count == 0 || array1.Count != array2.Count)
        {
            Debug.Log("EWS - Invalid input arrays!");
            return null;
        }

        int rows1 = array1.Count;
        int cols1 = array1[0].Count;
        int cols2 = array2[0].Count;

        List<List<double>> result = new List<List<double>>();

        for (int i = 0; i < rows1; i++)
        {
            List<double> newRow = new List<double>();
            for (int j = 0; j < cols1; j++)
            {
                double val1 = array1[i][j];
                double val2 = array2[i][j % cols2]; // Accessing corresponding element in array2

                newRow.Add(val1 - val2);
            }
            result.Add(newRow);
        }

        return result;
    }


    public List<List<double>> ElementwiseProduct(List<List<double>> array1, List<List<double>> array2)
    {
        if (array1 == null || array2 == null || array1.Count == 0 || array1.Count != array2.Count)
        {
            Debug.Log("EWP - Invalid input arrays!");
            return null;
        }

        int rows1 = array1.Count;
        int cols1 = array1[0].Count;
        int cols2 = array2[0].Count;

        List<List<double>> result = new List<List<double>>();

        for (int i = 0; i < rows1; i++)
        {
            List<double> newRow = new List<double>();
            for (int j = 0; j < cols1; j++)
            {
                double val1 = array1[i][j];
                double val2 = array2[i][j % cols2]; // Accessing corresponding element in array2

                newRow.Add(val1 * val2);
            }
            result.Add(newRow);
        }

        return result;
    }

    public List<List<double>> ElementwiseDivision(List<List<double>> array1, List<List<double>> array2)
    {
        if (array1 == null || array2 == null || array1.Count == 0 || array1.Count != array2.Count)
        {
            Debug.Log("EWD - Invalid input arrays!");
            return null;
        }

        int rows1 = array1.Count;
        int cols1 = array1[0].Count;
        int cols2 = array2[0].Count;

        List<List<double>> result = new List<List<double>>();

        for (int i = 0; i < rows1; i++)
        {
            List<double> newRow = new List<double>();
            for (int j = 0; j < cols1; j++)
            {
                double val1 = array1[i][j];
                double val2 = array2[i][j % cols2]; // Accessing corresponding element in array2

                newRow.Add(val1 / val2);
            }
            result.Add(newRow);
        }

        return result;
    }

    
    

    public List<List<double>> ScalarAddition(double n, List<List<double>> vector)
    {
        /**
            Adds a scalar value to each element of a 2D array.
            
            Args:
                n: Scalar value to be added to the elements of the vector
                vector: 2D array to which the scalar value is added

            Returns:
                A 2D array representing the addition of the scalar value to each element of the input vector
        **/

        if (vector == null)
        {
            Debug.Log("SA - Invalid input vector");
            return null; // Return null or handle error according to your needs
        }


        List<List<double>> result = new List<List<double>>();

        for (int i = 0; i < vector.Count; i++)
        {
            result.Add(new List<double>());
            for (int j = 0; j < vector[i].Count; j++)
            {
                result[i].Add(vector[i][j] + n);
            }
        }

        return result;
    }

    
    public List<List<double>> ScalarMultiplication(double n, List<List<double>> vector)
    {
        /**
            Multiplies a scalar value with each element of a 2D array.
            
            Args:
                n: Scalar value to be multiplied with the elements of the vector
                vector: 2D array to which the scalar value is multiplied

            Returns:
                A 2D array representing the multiplication of the scalar value with each element of the input vector
        **/

        if (vector == null)
        {
            Debug.Log("SM - Invalid input vector");
            return null; // Return null or handle error according to your needs
        }

        List<List<double>> result = new List<List<double>>();

        for (int i = 0; i < vector.Count; i++)
        {
            result.Add(new List<double>());
            for (int j = 0; j < vector[i].Count; j++)
            {
                result[i].Add(vector[i][j] * n);
            }
        }

        return result;
    }

    public List<List<double>> ScalarSubtraction(List<List<double>> vector, double n, bool reversed = false)
    {
        /**
            Subtracts a scalar value from each element of a 2D array or performs scalar subtraction from the given value.
            
            Args:
                vector: 2D array or scalar value for the subtraction operation
                n: Scalar value or 2D array used for subtraction
                reversed: If true, perform operation n - vector; if false, perform operation vector - n

            Returns:
                A 2D array representing the subtraction operation based on the specified order
        **/

        if (vector == null)
        {
            Debug.Log("SS - Invalid input vector");
            return null; // Return null or handle error according to your needs
        }

        List<List<double>> result = new List<List<double>>();

        if (reversed)
        {
            

        for (int i = 0; i < vector.Count; i++)
        {
            result.Add(new List<double>());
            for (int j = 0; j < vector[i].Count; j++)
            {
                result[i].Add(n - vector[i][j]);
            }
        }

        
        }
        else
        {
            for (int i = 0; i < vector.Count; i++)
        {
            result.Add(new List<double>());
            for (int j = 0; j < vector[i].Count; j++)
            {
                result[i].Add(vector[i][j] - n);
            }
        }
        }

        return result;
    }

    public List<List<double>> ScalarDivision(List<List<double>> vector, double n, bool reversed = false)
    {
        /**
            Divides each element of a 2D array by a scalar value or performs scalar division of the given value.
            
            Args:
                vector: 2D array or scalar value for the division operation
                n: Scalar value or 2D array used for division
                reversed: If true, perform operation n / vector; if false, perform operation vector / n

            Returns:
                A 2D array representing the division operation based on the specified order
        **/

        if (vector == null || Math.Abs(n) < double.Epsilon)
        {
            Debug.Log("SD - Invalid input vector or division by zero");
            return null; // Return null or handle error according to your needs
        }

        List<List<double>> result = new List<List<double>>();

        if (reversed)
        {
            

        for (int i = 0; i < vector.Count; i++)
        {
            result.Add(new List<double>());
            for (int j = 0; j < vector[i].Count; j++)
            {
                result[i].Add(n / vector[i][j]);
            }
        }

        
        }
        else
        {
            for (int i = 0; i < vector.Count; i++)
        {
            result.Add(new List<double>());
            for (int j = 0; j < vector[i].Count; j++)
            {
                result[i].Add(vector[i][j] / n);
            }
        }
        }

        return result;
    }
    
    public List<List<double>> ScalarPower(double n, List<List<double>> vector, bool reversed = false)
    {
        /**
            Exponentiates n by every element of array or exponentiates each element of a 2D array by a scalar value
            
            Args:
                vector: 2D array 
                n: Scalar value
                reversed: If true, perform operation vector**n; if false, perform operation n**vector

            Returns:
                A 2D array representing exponentiated version of it
        **/

        if (vector == null)
        {
            Debug.Log("SPw - Invalid input vector");
            return null; // Return null or handle error according to your needs
        }

        List<List<double>> result = new List<List<double>>();

        if (reversed)
        {
            for (int i = 0; i < vector.Count; i++)
            {
                result.Add(new List<double>());
                for (int j = 0; j < vector[i].Count; j++)
                {
                    result[i].Add(Math.Pow(n, vector[i][j]));
                }
            }
        }
        else
        {
            for (int i = 0; i < vector.Count; i++)
            {
                result.Add(new List<double>());
                for (int j = 0; j < vector[i].Count; j++)
                {
                    result[i].Add(Math.Pow(vector[i][j], n));
                }
            }
        }

        return result;
    }

    public List<List<double>> VectorisedExp(List<List<double>> array)
    {
         /**
            Exponentiates every element of the array (e**array)
            
            Args:
                vector: 2D array
            Returns:
                A 2D array representing exponentiated version of it
        **/

        List<List<double>> result = new List<List<double>>();

        for (int i = 0; i < array.Count; i++)
        {
            List<double> newRow = new List<double>();

            for (int j = 0; j < array[i].Count; j++)
            {
                newRow.Add(Math.Exp(array[i][j]));
            }

            result.Add(newRow);
        }

        return result;
    }

    
    public List<List<double>> VectorisedClip(List<List<double>> array, double min, double max)
    {
         /**
            Exponentiates every element of the array (e**array)
            
            Args:
                vector: 2D array
            Returns:
                A 2D array representing exponentiated version of it
        **/

        List<List<double>> result = new List<List<double>>();

        for (int i = 0; i < array.Count; i++)
        {
            List<double> newRow = new List<double>();

            for (int j = 0; j < array[i].Count; j++)
            {
                newRow.Add(Math.Clamp(array[i][j], min, max));
            }

            result.Add(newRow);
        }

        return result;
    }
    
    public List<List<double>> VectorisedLog(List<List<double>> array)
    {
         /**
            Exponentiates every element of the array (e**array)
            
            Args:
                vector: 2D array
            Returns:
                A 2D array representing exponentiated version of it
        **/

        List<List<double>> result = new List<List<double>>();

        for (int i = 0; i < array.Count; i++)
        {
            List<double> newRow = new List<double>();

            for (int j = 0; j < array[i].Count; j++)
            {
                newRow.Add(Math.Log(array[i][j]));
            }

            result.Add(newRow);
        }

        return result;
    }

    public List<List<double>> Max(List<List<double>> vector) {
        List<List<double>> result = new List<List<double>>();

        foreach(List<double> innerList in vector) {
            result.Add(new List<double>() {innerList.Max()});
        }

        return result;
    }

    public List<List<double>> ArgMax(List<List<double>> vector) {
        List<List<double>> result = new List<List<double>>();

        foreach(List<double> innerList in vector) {
            result.Add(new List<double>() {innerList.IndexOf(innerList.Max())});
        }

        return result;
    }

    public List<List<double>> TransposeMatrix(List<List<double>> matrix)
    {
        /**
            Transposes a 2D matrix (list of lists).

            Args:
                matrix: The 2D matrix to be transposed

            Returns:
                The transposed matrix
        **/

        if (matrix == null || matrix.Count == 0)
        {
            Debug.Log("TM - Matrix cannot be null or empty");
            return null; // Handle error or return null according to your needs
        }

        int rows = matrix.Count;
        int cols = matrix[0].Count;

        List<List<double>> transposedMatrix = new List<List<double>>();

        for (int j = 0; j < cols; j++)
        {
            transposedMatrix.Add(new List<double>());
            for (int i = 0; i < rows; i++)
            {
                transposedMatrix[j].Add(matrix[i][j]);
            }
        }

        return transposedMatrix;
    }
    
    public List<List<double>> SumArray(List<List<double>> arr, int axis = 0, bool keepDims = true)
    {
        if (arr == null || arr.Count == 0 || arr.Any(subList => subList == null || subList.Count == 0))
        {
            Debug.Log("SA - Array cannot be null or empty");
            return null; // Handle error or return null according to your needs
        }

        
        int rows = arr.Count;
        int cols = arr[0].Count;

        if (axis == 0) // Sum along columns (vertical sum)
        {
        List<double> sumResult = new List<double>();

        for (int j = 0; j < cols; j++)
        {
            double columnSum = 0;
            for (int i = 0; i < rows; i++)
            {
                columnSum += arr[i][j]; // Summing along the columns
            }
            sumResult.Add(columnSum);
        }

        if (keepDims)
        {
            return new List<List<double>> { sumResult };
            
        }
        else
        {
            return sumResult.Select(val => new List<double> { val }).ToList();
        }
    }
        else if (axis == 1) // Sum along rows (horizontal sum)
        {
            List<List<double>> sumResult = new List<List<double>>();

            for (int i = 0; i < rows; i++)
            {
                double rowSum = arr[i].Sum();
                if (keepDims)
                {
                    sumResult.Add(new List<double> { rowSum });
                }
                else
                {
                    sumResult.Add(new List<double> { rowSum });
                }
            }

            return sumResult;
        }
        else
        {
            Debug.Log("SA - Invalid axis. Use axis = 0 for columns or axis = 1 for rows");
            return null; // Handle error or return null according to your needs
        }
    }
    
    public List<List<double>> VectorisedTanhH(List<List<double>> arr) {
        for (int i = 0; i < arr.Count; i++)
        {
            for (int j = 0; j < arr[i].Count; j++)
            {
                arr[i][j] = Math.Tanh(arr[i][j]);
            }
        }

        return arr;
    }

    public List<List<double>> Mean(int axis, List<List<double>> arr) {
        
        if (axis != 0 && axis != 1) {
            Debug.Log("M - Invalid axis provided (0 or 1)");
            return null;
            
        }
        
        return ScalarDivision(SumArray(arr, axis, true), arr.Count);
    }


    public List<List<double>> BroadcastedAddition(List<List<double>> array1, List<List<double>> array2)
    {
        if (array1 == null || array2 == null || array1.Count == 0 || array2.Count != 1)
        {
            Debug.Log("BA - Invalid input arrays!");
            return null;
        }

        int rows1 = array1.Count;
        int cols1 = array1[0].Count;
        int cols2 = array2[0].Count;

        List<List<double>> result = new List<List<double>>();

        for (int i = 0; i < rows1; i++)
        {
            List<double> newRow = new List<double>();
            for (int j = 0; j < cols1; j++)
            {
                double val1 = array1[i][j];
                double val2 = array2[0][j % cols2]; // Repeating values from array2

                newRow.Add(val1 + val2);
            }
            result.Add(newRow);
        }

        return result;
    }    

    public List<List<double>> BroadcastedSubtraction(List<List<double>> array1, List<List<double>> array2)
    {
        if (array1 == null || array2 == null || array1.Count == 0 || array2.Count != 1)
        {
            Debug.Log("BS - Invalid input arrays!");
            return null;
        }

        int rows1 = array1.Count;
        int cols1 = array1[0].Count;
        int cols2 = array2[0].Count;

        List<List<double>> result = new List<List<double>>();

        for (int i = 0; i < rows1; i++)
        {
            List<double> newRow = new List<double>();
            for (int j = 0; j < cols1; j++)
            {
                double val1 = array1[i][j];
                double val2 = array2[0][j % cols2]; // Repeating values from array2

                newRow.Add(val1 - val2);
            }
            result.Add(newRow);
        }

        return result;
    }    



    public List<List<double>> BroadcastedProduct(List<List<double>> array1, List<List<double>> array2)
    {
        if (array1 == null || array2 == null || array1.Count == 0 || array2.Count != 1)
        {
            Debug.Log("BP - Invalid input arrays!");
            return null;
        }

        int rows1 = array1.Count;
        int cols1 = array1[0].Count;
        int cols2 = array2[0].Count;

        List<List<double>> result = new List<List<double>>();

        for (int i = 0; i < rows1; i++)
        {
            List<double> newRow = new List<double>();
            for (int j = 0; j < cols1; j++)
            {
                double val1 = array1[i][j];
                double val2 = array2[0][j % cols2]; // Repeating values from array2

                newRow.Add(val1 * val2);
            }
            result.Add(newRow);
        }

        return result;
    }    


    public List<List<double>> BroadcastedDivision(List<List<double>> array1, List<List<double>> array2)
    {
        if (array1 == null || array2 == null || array1.Count == 0 || array2.Count != 1)
        {
            Debug.Log("BD - Invalid input arrays!");
            return null;
        }

        int rows1 = array1.Count;
        int cols1 = array1[0].Count;
        int cols2 = array2[0].Count;

        List<List<double>> result = new List<List<double>>();

        for (int i = 0; i < rows1; i++)
        {
            List<double> newRow = new List<double>();
            for (int j = 0; j < cols1; j++)
            {
                double val1 = array1[i][j];
                double val2 = array2[0][j % cols2]; // Repeating values from array2

                newRow.Add(val1 / val2);
            }
            result.Add(newRow);
        }

        return result;
    }    
    
    public List<List<double>> DeepCopy(List<List<double>> arr)
    {
        if (arr == null || arr.Count == 0)
        {
            Debug.Log("DC - Array cannot be null or empty");
            return arr; // Return or handle error according to your needs
        }

        var copy = new List<List<double>>();

        foreach (var sublist in arr)
        {
            var sublistCopy = new List<double>(sublist);
            copy.Add(sublistCopy);
        }

        return copy;
    }

    public void DebugArray(List<List<double>> arr)
    {
        /**
            Displays a 2D array in a readable format with each row on a new line but elements inline within each row
            
            Args:
                arr: The 2D array to be displayed
        **/

        string output = "[ ";

        for (int i = 0; i < arr.Count; i++)
        {
            output += "[";
            for (int j = 0; j < arr[i].Count; j++)
            {
                output += arr[i][j].ToString("0.00");
                if (j < arr[i].Count - 1)
                {
                    output += ", ";
                }
            }
            output += "]";
            if (i < arr.Count - 1)
            {
                output += ",\n  ";
            }
        }
        output += " ]";

        Debug.Log(output);
    }

    public void ArrayShape(List<List<double>> arr)
    {
        if (arr == null || arr.Count == 0)
        {
            Debug.Log("AS - Array cannot be null or empty");
            return; // Return or handle error according to your needs
        }

        int rows = arr.Count;
        int cols = arr[0].Count;

        Debug.Log($"({rows}, {cols})");
    }
}


interface IActivationFuntion {
    
    public void Forward(List<List<double>> inputs);

    public void Backward(List<List<double>> dvalues);
}



class Activation_ReLU : IActivationFuntion {

    public List<List<double>> inputs = new List<List<double>> ();
    public List<List<double>> output = new List<List<double>> ();
    public List<List<double>> dinputs = new List<List<double>> ();
    private readonly NumCs nc = new NumCs();

    public void Forward(List<List<double>> inputs) {
        this.inputs = inputs;
        output = nc.Maximum(0, inputs);
    }

    public void Backward(List<List<double>> dvalues) {
        dinputs = nc.Maximum(0, dvalues);
    }
}

class Activation_TanH : IActivationFuntion {
    
    public List<List<double>> inputs = new List<List<double>> ();
    public List<List<double>> output = new List<List<double>> ();
    public List<List<double>> dinputs = new List<List<double>> ();

    private readonly NumCs nc = new NumCs();

    public void Forward(List<List<double>> inputs) {
        /**
        TanH function

        f(x) = tanh(x)
        
        **/

        this.inputs = inputs;


        output = nc.VectorisedTanhH(this.inputs);
        
    }

    public void Backward(List<List<double>> dvalues) {
        /**
            f'(x) = sech^2(x)
        **/

        // dvalues: gradients from the next layer or loss function

        List<List<double>> derivative = new List<List<double>>();

        // Compute derivative for each element in the output using the derivative formula
        for (int i = 0; i < output.Count; i++)
        {
            List<double> row = new List<double>();
            for (int j = 0; j < output[i].Count; j++)
            {
                row.Add(1 - output[i][j] * output[i][j]);
            }
            derivative.Add(row);
        }

        // Element-wise multiplication of derivative with the incoming gradients (dvalues)
        // This gives the gradients with respect to the input
        // This result can then be used in the backpropagation algorithm

        // Example of element-wise multiplication (You may need to modify this according to your library)
        for (int i = 0; i < dvalues.Count; i++)
        {
            for (int j = 0; j < dvalues[i].Count; j++)
            {
                dvalues[i][j] *= derivative[i][j];
            }
        }

        dinputs = dvalues; 
    }
}

class Activation_Softmax : IActivationFuntion {

    public List<List<double>> inputs = new List<List<double>> ();
    public List<List<double>> output = new List<List<double>> ();
    public List<List<double>> dinputs = new List<List<double>> ();

    private readonly NumCs nc = new NumCs();

    public void Forward(List<List<double>> inputs) {
        this.inputs = inputs;

        List<List<double>> exponientiated = nc.VectorisedExp(nc.BroadcastedSubtraction(inputs, nc.TransposeMatrix(nc.Max(inputs))));
        
       
        output = nc.BroadcastedDivision(exponientiated, nc.TransposeMatrix(nc.SumArray(exponientiated, 1, true)));
    }

    public void Backward(List<List<double>> dvalues) {
        Debug.Log("Inneficient ...");
        return;
    }
}

interface ILoss {
    

    public void Forward(List<List<double>> ypred, List<List<double>> ytrue);
    public void Backward(List<List<double>> ytrue);

}

class Loss_MSE : ILoss {

    
    public List<List<double>> inputs = new List<List<double>> ();
    public List<List<double>> output = new List<List<double>> ();
    public List<List<double>> dinputs = new List<List<double>> ();

    private NumCs nc = new NumCs();


    public void Forward(List<List<double>> ypred, List<List<double>> ytrue) {
        inputs = ypred;

        List<List<double>> error = nc.ElementwiseAddition(ypred, nc.ScalarMultiplication(-1, ytrue));
        
        List<List<double>> squaredError = nc.ElementwiseProduct(error, error);
        
        
        output = nc.ScalarDivision(nc.SumArray(nc.SumArray(squaredError, 0, true), 1, true), (squaredError[0].Count * squaredError.Count));
    }


    public void Backward(List<List<double>> ytrue)
    {
        int outputs = output[0].Count;

        dinputs = nc.ScalarDivision(nc.ScalarMultiplication(2, nc.ElementwiseAddition(inputs, nc.ScalarMultiplication(-1, ytrue))), outputs, false);
    }

}

class Loss_CategoricalCrossEntropy : ILoss {
    public List<List<double>> inputs = new List<List<double>> ();
    public List<List<double>> output = new List<List<double>> ();
    public List<List<double>> dinputs = new List<List<double>> ();

    private NumCs nc = new NumCs();
    
    public void Forward(List<List<double>> ypred, List<List<double>> ytrue) {
        
        List<List<double>> ypredClipped = nc.VectorisedClip(ypred, 1e-7, 1 - 1e-7);
        
        List<List<double>> correct_confidences = nc.SumArray(nc.ElementwiseProduct(ypredClipped, ytrue), 1);
       
        
        output = nc.ScalarMultiplication(-1, nc.VectorisedLog(correct_confidences));
    }

    public void Backward(List<List<double>> ytrue) {
        Debug.Log("inneficient...");
    }

    public List<List<double>> Calculate(List<List<double>> output, List<List<double>> ytrue) {
        Forward(output, ytrue);
        return nc.Mean(0, this.output);
    }
}


class Activation_Softmax_Loss_CategoricalCrossEntropy {
    public List<List<double>> inputs = new List<List<double>> ();
    public List<List<double>> output = new List<List<double>> ();
    public List<List<double>> dinputs = new List<List<double>> ();

    private NumCs nc = new NumCs();

    Activation_Softmax activationFunction = new Activation_Softmax();
    public Loss_CategoricalCrossEntropy loss = new Loss_CategoricalCrossEntropy();

    public List<List<double>> Forward(List<List<double>> inputs, List<List<double>> ytrue) {
        activationFunction.Forward(inputs);
        output = activationFunction.output;
        return loss.Calculate(output, ytrue);
    }

    public void Backward(List<List<double>> dvalues, List<List<double>> ytrue) {
        int samples = dvalues.Count;

        dinputs = nc.DeepCopy(dvalues);

        var trueIndexes = nc.TransposeMatrix(nc.ArgMax(ytrue))[0];

        for (int i = 0; i < trueIndexes.Count; i++)
        {
            dinputs[i][(int)trueIndexes[i]] -= 1;
        }

        dinputs = nc.ScalarDivision(dinputs, samples);


    }
}

class Layer_Dense {

    public List<List<double>> inputs;
    public List<List<double>> output;
    public List<List<double>> dinputs;
    public List<List<double>> weights;
    public List<List<double>> dweights;
    public List<List<double>> biases;
    public List<List<double>> dbiases;

    

    private readonly NumCs nc = new NumCs();

    public Layer_Dense(int nInputs, int nOutputs) {
        
        weights = nc.Rand2D(nInputs, nOutputs);
        
        biases = nc.Full(n: 0, 1, nOutputs);
    }

    public void Forward(List<List<double>> inputs) {
        this.inputs = inputs;
        
        output = nc.BroadcastedAddition(nc.MatMul(inputs, weights), biases);
    }

    public void Backward(List<List<double>> dvalues) {
     
        dweights = nc.MatMul(nc.TransposeMatrix(inputs), dvalues);
        
        dbiases = nc.SumArray(dvalues, 0, true);
        dinputs = nc.MatMul(dvalues, nc.TransposeMatrix(weights));
        
    }
}

interface Optimiser {
    double learningRate {get; set;}
    

    public void PreUpdatePararms() {
        learningRate = learningRate; //add decay logic
    }
    
    public void UpdateParams(Layer_Dense layer);

    public void PostUpdateParams() {

        // iterations += 1;
        return;
    }
}


class Optimiser_SGD : Optimiser {
    double _learningRate;
    public double learningRate {
        get => _learningRate;
        set => _learningRate = value;
    }

    private NumCs nc = new NumCs();

    public Optimiser_SGD(double learningRate) {
        this.learningRate = learningRate;
    }

    public void UpdateParams(Layer_Dense layer) {
        List<List<double>> weight_changes = nc.ScalarMultiplication(-learningRate, layer.dweights);
        List<List<double>> bias_changes = nc.ScalarMultiplication(-learningRate, layer.dbiases);
        
        layer.weights = nc.ElementwiseAddition(layer.weights, weight_changes);
        layer.biases = nc.ElementwiseAddition(layer.biases, bias_changes);
        
    }



}