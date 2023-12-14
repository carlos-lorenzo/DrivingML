using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

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
                result[i].Add(Math.Max(n, arr[i][j]));
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
                result[i].Add((double)rand.NextDouble() * 2 - 1);
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

    public void DebugArrayShape(List<List<double>> arr)
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

    public List<int> ArrayShape(List<List<double>> arr)
    {
        if (arr == null || arr.Count == 0)
        {
            Debug.Log("AS - Array cannot be null or empty");
            return null; // Return or handle error according to your needs
        }

        int rows = arr.Count;
        int cols = arr[0].Count;

        
        return new List<int>() {rows, cols};
    }
}

