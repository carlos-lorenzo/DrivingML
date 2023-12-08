using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime;
using Unity.Mathematics;


public class NeuralNetwork : MonoBehaviour
{
    public List<int> size1;
    public List<int> size2;
    void Start() {

        NumCs nc = new NumCs();

        Layer_Dense layer1 = new Layer_Dense(size1[0], size1[1]);
        Activation_ReLU activation1 = new Activation_ReLU();
        
        Layer_Dense layer2 = new Layer_Dense(size2[0], size2[1]);
        Activation_TanH activation2 = new Activation_TanH();

        List<List<double>> inputs = nc.Rand2D(1, size1[0]);
        

        layer1.Forward(inputs);
        
        activation1.Forward(layer1.output);

        layer2.Forward(activation1.output);
        activation2.Forward(layer2.output);


        nc.DebugArray(activation2.output);

        
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
            Debug.LogError("Array cannot be null!");
            return null; // Return null or handle error according to your needs
        }

        
        
        
        for (int i = 0; i < arr.Count; i++)
        {
            
            for (int j = 0; j < arr[i].Count; j++)
            {
                arr[i][j] = Math.Max(0, arr[i][j]);
            }
            
        }

        return arr;
    }

    public List<List<double>> Rand2D(int dim1, int dim2) {
        /**  
            Returns a 2D array with given size, filled with random values
        **/

        if (dim1 <= 0 || dim2 <= 0)
        {
            Console.WriteLine("Invalid size provided!");
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
            Debug.LogError("Invalid size provided!");
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
            Console.WriteLine("Matrices cannot be null");
            return null; // Handle error or return null according to your needs
        }

        int rowsA = matrix1.Count;
        int colsA = matrix1[0].Count;
        int rowsB = matrix2.Count;
        int colsB = matrix2[0].Count;

        if (colsA != rowsB)
        {
            Console.WriteLine("Cannot multiply matrices: Incompatible dimensions");
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


    public List<List<double>> VectorisedSum(List<List<double>> array1, List<List<double>> array2)
    {
        /**
            Element-wise addition of two 2D arrays.
            
            Args:
                array1: First 2D array for addition
                array2: Second 2D array for addition, can have (a, b) or (1, b) shape

            Returns:
                A 2D array representing the element-wise sum of array1 and array2 
                with the shape determined by the largest first dimension
        **/

        if (array1 == null || array2 == null)
        {
            Debug.LogError("Invalid input arrays!");
            return null; // Return null or handle error according to your needs
        }

        int rows1 = array1.Count;
        int rows2 = array2.Count;
        int cols1 = array1[0].Count;
        int cols2 = array2[0].Count;

        int maxRows = Math.Max(rows1, rows2);
        int maxCols = Math.Max(cols1, cols2);

        List<List<double>> result = new List<List<double>>();

        for (int i = 0; i < maxRows; i++)
        {
            List<double> newRow = new List<double>();
            for (int j = 0; j < maxCols; j++)
            {
                double val1 = (i < rows1 && j < cols1) ? array1[i][j] : 0;
                double val2 = (i < rows2 && j < cols2) ? array2[i][j] : 0;

                newRow.Add(val1 + val2);
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
            Debug.LogError("Invalid input vector");
            return null; // Return null or handle error according to your needs
        }

        for (int i = 0; i < vector.Count; i++)
        {
            for (int j = 0; j < vector[i].Count; j++)
            {
                vector[i][j] += n;
            }
        }

        return vector;
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
            Debug.LogError("Invalid input vector");
            return null; // Return null or handle error according to your needs
        }

        for (int i = 0; i < vector.Count; i++)
        {
            for (int j = 0; j < vector[i].Count; j++)
            {
                vector[i][j] *= n;
            }
        }

        return vector;
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
            Debug.LogError("Invalid input vector");
            return null; // Return null or handle error according to your needs
        }

        if (reversed)
        {
            for (int i = 0; i < vector.Count; i++)
            {
                for (int j = 0; j < vector[i].Count; j++)
                {
                    vector[i][j] = n - vector[i][j];
                }
            }
        }
        else
        {
            for (int i = 0; i < vector.Count; i++)
            {
                for (int j = 0; j < vector[i].Count; j++)
                {
                    vector[i][j] -= n;
                }
            }
        }

        return vector;
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
            Debug.LogError("Invalid input vector or division by zero");
            return null; // Return null or handle error according to your needs
        }

        if (reversed)
        {
            for (int i = 0; i < vector.Count; i++)
            {
                for (int j = 0; j < vector[i].Count; j++)
                {
                    vector[i][j] = n / vector[i][j];
                }
            }
        }
        else
        {
            for (int i = 0; i < vector.Count; i++)
            {
                for (int j = 0; j < vector[i].Count; j++)
                {
                    vector[i][j] /= n;
                }
            }
        }

        return vector;
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
            Debug.LogError("Invalid input vector");
            return null; // Return null or handle error according to your needs
        }

        if (reversed)
        {
            for (int i = 0; i < vector.Count; i++)
            {
                for (int j = 0; j < vector[i].Count; j++)
                {
                    vector[i][j] = Math.Pow(n, vector[i][j]);
                }
            }
        }
        else
        {
            for (int i = 0; i < vector.Count; i++)
            {
                for (int j = 0; j < vector[i].Count; j++)
                {
                    vector[i][j] = Math.Pow(vector[i][j], n);
                }
            }
        }

        return vector;
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

    public List<List<double>> VectorisedProduct(List<List<double>> array1, List<List<double>> array2)
    {
        /**
        Element-wise multiplication of two 2D arrays. Can have (a, b) or (1, b)
        
        Args:
            array1: First 2D array for multiplication
            array2: Second 2D array for multiplication

        Returns:
            A 2D array representing the element-wise multiplication of array1 and array2 
            with the shape determined by the largest first dimension
        **/

        if (array1 == null || array2 == null)
        {
            Console.WriteLine("Invalid input arrays!");
            return null; // Return null or handle error according to your needs
        }

        int rows1 = array1.Count;
        int rows2 = array2.Count;

        
        int maxRows = Math.Max(rows1, rows2);
        Console.WriteLine(maxRows);
        List<List<double>> result = new List<List<double>>();

        for (int i = 0; i < maxRows; i++)
        {
            int cols1 = (i < rows1) ? array1[i].Count : 0;
            int cols2 = (i < rows2) ? array2[i].Count : 0;
            int maxCols = Math.Max(cols1, cols2);
        
            result.Add(new List<double>());

            for (int j = 0; j < maxCols; j++)
            {
                double val1 = (i < rows1 && j < cols1) ? array1[i][j] : 0;
                double val2 = (i < rows2 && j < cols2) ? array2[i][j] : 0;

                result[i].Add(val1 * val2);
            }
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
            Debug.Log("Matrix cannot be null or empty");
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
            Debug.Log("Array cannot be null or empty");
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
                    columnSum += arr[i][j];
                }
                sumResult.Add(columnSum);
            }

            if (keepDims)
            {
                return sumResult.Select(val => new List<double> { val }).ToList();
            }
            else
            {
                return new List<List<double>> { sumResult };
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
            Debug.Log("Invalid axis. Use axis = 0 for columns or axis = 1 for rows");
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

   public List<List<double>> Mean(List<List<double>> arr) {
        return ScalarDivision(SumArray(arr, 0, true), arr[0].Count);
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
            Debug.LogError("Array cannot be null or empty");
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
        dinputs = nc.Maximum(1, nc.Maximum(0, dvalues));
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


interface ILoss {
    //List<List<double>> inputs;
    public void Forward(List<List<double>> ypred, List<List<double>> ytrue){
        this.inputs = ypred;
    }
    public void Calculate(List<List<double>> ypred, List<List<double>> ytrue) {
        Forward(ypred, ytrue);
        //return new NumCs().Mean(this.output);
    }
}

class Loss_PolicyLoss : ILoss {
    public void Forward() {

    }

    public void Backward() {
        
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
        output = nc.VectorisedSum(nc.MatMul(inputs, weights), biases);
    }

    public void Backward(List<List<double>> dvalues) {
        dweights = nc.MatMul(nc.TransposeMatrix(this.inputs), dvalues);
        dbiases = nc.SumArray(dvalues, 0, false);
        dinputs = nc.MatMul(dvalues, nc.TransposeMatrix(weights));
        
    }
}