using UnityEngine;
using System.Collections.Generic;




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

public class Layer_Dense {

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