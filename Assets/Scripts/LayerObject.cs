using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

[CreateAssetMenu(fileName = "LayerObject", menuName = "ScriptableObjects/LayerObjectScriptableObject", order = 2)]
public class LayerObject : ScriptableObject
{

    public int inputFeatures;
    public int outputFeatures;

    public enum ActivationFunction {
        Activation_ReLU,
        Activation_Softmax,
        Activation_TanH,
        Activation_Softmax_Loss_CategoricalCrossEntropy
    }

    private NumCs nc = new NumCs();
    
    public bool hasLoss;

    public Layer_Dense layer;

    public ActivationFunction activationFunction;

    
    public void Initialise() {
        layer = new Layer_Dense(inputFeatures, outputFeatures);
    }

    public List<List<double>> Forward(List<List<double>> inputs, List<List<double>> ytrue ) {
        layer.Forward(inputs);
        

        switch (activationFunction) {
            case ActivationFunction.Activation_ReLU:
                Activation_ReLU relu = new Activation_ReLU();
                relu.Forward(layer.output);
                return relu.output;
               
            case ActivationFunction.Activation_Softmax:
                Activation_Softmax softmax = new Activation_Softmax();
                softmax.Forward(layer.output);
                return softmax.output;
                
            case ActivationFunction.Activation_TanH:
                Activation_TanH tanh = new Activation_TanH();
                tanh.Forward(layer.output);
                return tanh.output;
                
            case ActivationFunction.Activation_Softmax_Loss_CategoricalCrossEntropy:
                Activation_Softmax_Loss_CategoricalCrossEntropy softmaxCCE = new Activation_Softmax_Loss_CategoricalCrossEntropy();
                softmaxCCE.Forward(layer.output, ytrue);
                return softmaxCCE.output;
               
        }

        return inputs;
    }

}
