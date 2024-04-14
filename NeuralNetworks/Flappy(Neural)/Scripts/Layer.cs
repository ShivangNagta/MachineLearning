using System;
using Unity.VisualScripting;
public class Layer
{
    int numLeftNodes, numRightNodes;
    double[,] weights;
    double[] biases;
    
    public Layer (int numLeftNodes, int numRightNodes)
    {
        this.numLeftNodes = numLeftNodes;
        this.numRightNodes = numRightNodes;
        weights = new double[numLeftNodes, numRightNodes];
        biases = new double[numRightNodes];
    }

    public double[] Outputs(double[] inputs)
    {
        double[] Activated = new double[numRightNodes];

        for (int right = 0; right < numRightNodes; right++)
        {
            double weightedInput = biases[right];
            for (int left = 0; left < numLeftNodes; left++)
            {
                weightedInput += inputs[left] * weights[left, right];
            }
            Activated[right] = ActivationFunction(weightedInput);
        }
        return Activated;
    }

    double ActivationFunction(double weightedInput)
    {
        return 1/ (1 + Math.Exp(-weightedInput));
    }

    public double Mse(double outputActivated, double expectedOutput)
    {
        double error = outputActivated - expectedOutput;
        return error * error;
    }
    
        
}