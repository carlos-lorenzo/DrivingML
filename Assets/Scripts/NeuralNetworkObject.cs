using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NeuralNetwork", menuName = "ScriptableObjects/NeuralNetworkScriptableObject", order = 1)]
public class NeuralNetworkObject : ScriptableObject
{
    private NumCs nc = new NumCs();

    public List<LayerObject> layers;
    

    public List<List<double>> ForwardPass(List<List<double>> X, List<List<double>> ytrue) {
        /**
        layer_input = X
        
        for layer, activation_function in zip(self.layers[:-1], self.activation_functions):
            layer.forward(layer_input)
            
            
            activation_function.forward(layer.output)
                
            layer_input = activation_function.output
        
        self.layers[-1].forward(layer_input)
        self.loss = self.activation_loss.forward(inputs=self.layers[-1].output, y_true=y)
        self.predictions = np.argmax(self.activation_loss.activation_function.output, axis=1)
        
        **/

        var layerInput = X;

        foreach (LayerObject layer in layers) {
            

            layerInput = layer.Forward(layerInput, ytrue);
            if (layer.hasLoss) {
                break;
            }
        }
        nc.DebugArray(layerInput);

        return layerInput;
    }
}
