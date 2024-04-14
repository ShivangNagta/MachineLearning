public class NeuralNetwork
{
    Layer[] layers;

    public NeuralNetwork(params int[] layerSizes)
    {
        layers = new Layer[layerSizes.Length - 1];
        for (int i = 0; i < layers.Length; i++)
        {
            layers[i] = new Layer(layerSizes[i], layerSizes[i + 1]);
        }
    }

    double[] Outputs(double[] inputs)
    {
        foreach (Layer layer in layers)
        {
            inputs = layer.Outputs(inputs);
        }

        return inputs;
    }

    // double Cost(DataPoint datapoint)
    // {
    //     double[] outputs = Outputs((datapoint.inputs));
    //     Layer outputLayer = layers[layers.Length - 1];
    //     double cost = 0;
    //
    //     for (int outNode = 0; outNode < outputs.Length; outNode++)
    //     {
    //         cost += outputLayer.Mse(outputs[outNode], datapoint.expectedOutputs[outNode]);
    //     }
    //
    //     return cost;
    // }

    // double Cost(DataPoint[] data)
    // {
    //     double totalCost = 0;
    //     foreach (DataPoint dataPoint in data)
    //     {
    //         totalCost += Cost(dataPoint);
    //     }
    //
    //     return totalCost / data.Length;
    // }
    
}
