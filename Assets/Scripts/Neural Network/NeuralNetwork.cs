using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NeuralNetwork
{
    [System.Serializable]
    public class Neuron
    {
        public NeuronConnection[] neuronConnections;
        public double value;
        public double bias;

        public Neuron()
        {
            neuronConnections = new NeuronConnection[0];
        }

        public Neuron(Neuron neuronToCopy)
        {
            bias = neuronToCopy.bias;
            value = neuronToCopy.value;
            neuronConnections = new NeuronConnection[neuronToCopy.neuronConnections.Length];
        }

        public Neuron(NeuronSave neuronToCopy)
        {
            bias = neuronToCopy.bias;
            value = 0;
            neuronConnections = new NeuronConnection[neuronToCopy.neuronConnections.Length];
        }

        public void Randomize()
        {
            bias = Random.Range(-8f, 8f);
        }

        public void SetConnections(NeuronConnection[] neuronConnections)
        {
            this.neuronConnections = neuronConnections;
        }
    }

    [System.Serializable]
    public class NeuronConnection
    {
        public Neuron previousNeuron;
        public double weight;

        public NeuronConnection(Neuron previousNeuron)
        {
            this.previousNeuron = previousNeuron;
        }

        public void Randomize()
        {
            weight = Random.Range(-1f, 1f);
        }

        public NeuronConnection(Neuron previousNeuron, NeuronConnection connectionToCopy)
        {
            this.previousNeuron = previousNeuron;
            weight = connectionToCopy.weight;
        }

        public NeuronConnection(Neuron previousNeuron, double weight)
        {
            this.previousNeuron = previousNeuron;
            this.weight = weight;
        }
    }

    Neuron[][] neurons;
    int[] layers;

    public NeuralNetwork(int[] layers)
    {
        neurons = new Neuron[layers.Length][];
        this.layers = layers;

        // Create neurons
        for (int i = 0; i < layers.Length; i++)
        {
            neurons[i] = new Neuron[layers[i]];
            for (int n = 0; n < layers[i]; n++)
            {
                neurons[i][n] = new Neuron();
                neurons[i][n].Randomize();

                if (i == 0)
                    continue;

                // Create neuron connections
                NeuronConnection[] connections = new NeuronConnection[layers[i - 1]];
                for (int p = 0; p < layers[i - 1]; p++)
                {
                    connections[p] = new NeuronConnection(neurons[i - 1][p]);
                    connections[p].Randomize();
                }

                neurons[i][n].neuronConnections = connections;
            }
        }
    }

    public NeuralNetwork(NeuralNetwork networkToCopy)
    {
        layers = networkToCopy.GetLayers();

        neurons = new Neuron[layers.Length][];

        // Create neurons
        for (int i = 0; i < layers.Length; i++)
        {
            neurons[i] = new Neuron[layers[i]];
            for (int n = 0; n < layers[i]; n++)
            {
                Neuron neuronToCopy = networkToCopy.GetNeuron(i, n);
                neurons[i][n] = new Neuron(neuronToCopy);

                if (i == 0)
                    continue;

                // Create neuron connections
                NeuronConnection[] connections = new NeuronConnection[layers[i - 1]];
                for (int p = 0; p < layers[i - 1]; p++)
                {
                    connections[p] = new NeuronConnection(neurons[i - 1][p], neuronToCopy.neuronConnections[p]);
                }

                neurons[i][n].neuronConnections = connections;
            }
        }
    }

    public NeuralNetwork(string[] networkToCopy)
    {
        // Line 3: layers
        string[] layerStrings = SeparateByComma(networkToCopy[2]);
        layers = new int[layerStrings.Length];
        for (int i = 0; i < layers.Length; i++)
            layers[i] = int.Parse(layerStrings[i]);

        // Neurons
        neurons = new Neuron[layers.Length][];

        // Create neurons
        int copyIndex = 2;
        for (int i = 0; i < layers.Length; i++)
        {
            neurons[i] = new Neuron[layers[i]];
            for (int n = 0; n < layers[i]; n++)
            {
                copyIndex++;

                NeuronSave neuronToCopy = JsonUtility.FromJson<NeuronSave>(networkToCopy[copyIndex]);
                neurons[i][n] = new Neuron(neuronToCopy);

                if (i == 0)
                    continue;

                // Create neuron connections
                NeuronConnection[] connections = new NeuronConnection[layers[i - 1]];
                for (int p = 0; p < layers[i - 1]; p++)
                {
                    connections[p] = new NeuronConnection(neurons[i - 1][p], neuronToCopy.neuronConnections[p]);
                }

                neurons[i][n].neuronConnections = connections;
            }
        }
    }

    public double[] RunNeuralNetwork(double[] input)
    {
        // Check if input matches first layer
        if (input.Length != neurons[0].Length)
            return new double[0];

        // Set first layer to inputs
        for (int i = 0; i < input.Length; i++)
        {
            neurons[0][i].value = input[i];
        }

        // Calculate rest of layers
        for (int i = 1; i < neurons.Length; i++)
        {
            for (int n = 0; n < neurons[i].Length; n++)
            {
                // Calculate sum of neuron connections
                double sum = 0;
                for (int c = 0; c < neurons[i][n].neuronConnections.Length; c++)
                {
                    sum += neurons[i][n].neuronConnections[c].previousNeuron.value * neurons[i][n].neuronConnections[c].weight;
                }

                // Calculate bias
                sum += neurons[i][n].bias;

                // Calculate value with sigmoid function
                double value = HyperbolicTangentActivation(sum);

                // Set neuron value
                neurons[i][n].value = value;
            }
        }

        // Return last layer
        double[] output = new double[neurons[neurons.Length - 1].Length];

        for (int i = 0; i < neurons[neurons.Length - 1].Length; i++)
        {
            output[i] = neurons[neurons.Length - 1][i].value;
        }

        return output;
    }

    double SigmoidActivation(double x)
    {
        return 1 / (1 + System.Math.Pow(System.Math.E, -x));
    }

    double HyperbolicTangentActivation(double x)
    {
        return (System.Math.Pow(System.Math.E, x) - System.Math.Pow(System.Math.E, -x)) /
            (System.Math.Pow(System.Math.E, x) + System.Math.Pow(System.Math.E, -x));
    }

    public void Mutate(float mutationRate, double weightMutationStrength, double biasMutationStrength)
    {
        // Init Random
        System.Random random = new System.Random();
        
        // Run through the layers
        for (int i = 0; i < neurons.Length; i++)
        {
            for (int n = 0; n < neurons[i].Length; n++)
            {
                // Mutate the bias
                if (Random.value <= mutationRate)
                    neurons[i][n].bias += (random.NextDouble() * (biasMutationStrength * 2) - biasMutationStrength);

                // Run through all connections
                for (int c = 0; c < neurons[i][n].neuronConnections.Length; c++)
                {
                    // Mutate the weight
                    if (Random.value <= mutationRate)
                        neurons[i][n].neuronConnections[c].weight = System.Math.Clamp(neurons[i][n].neuronConnections[c].weight + (random.NextDouble() * (weightMutationStrength * 2) - weightMutationStrength), -1, 1);
                }
            }
        }
    }

    public int GetNumLayers()
    {
        return neurons.Length;
    }

    public Neuron[] GetLayer(int layer)
    {
        return neurons[layer];
    }
    
    public Neuron GetNeuron(int layer, int neuron)
    {
        return neurons[layer][neuron];
    }

    public int[] GetLayers()
    {
        return layers;
    }

    [System.Serializable]
    public struct NeuronSave
    {
        public double[] neuronConnections;
        public double bias;

        public NeuronSave(Neuron neuron)
        {
            neuronConnections = new double[neuron.neuronConnections.Length];
            for (int i = 0; i < neuronConnections.Length; i++)
                neuronConnections[i] = neuron.neuronConnections[i].weight;
            bias = neuron.bias;
        }
    }

    public string[] OutputNetwork(float fitness, int generation)
    {
        List<string> output = new List<string>();

        // Line 1: fitness
        output.Add(fitness.ToString());

        // Line 2: Generation
        output.Add(generation.ToString());

        // Line 3: layers
        string line2 = "";
        for (int i = 0; i < layers.Length; i++)
        {
            line2 += layers[i] + ",";
        }
        output.Add(line2);

        // Rest of the lines: neurons
        for (int l = 0; l < layers.Length; l++)
        {
            for (int n = 0; n < neurons[l].Length; n++)
            {
                NeuronSave neuronSave = new NeuronSave(neurons[l][n]);
                output.Add(JsonUtility.ToJson(neuronSave));
            }
        }

        return output.ToArray();
    }

    public static float GetFitnessFromFile(string[] fileContents)
    {
        return float.Parse(fileContents[0]);
    }

    public static int GetGenerationFromFile(string[] fileContents)
    {
        return int.Parse(fileContents[1]);
    }

    public string[] SeparateByComma(string stringToSeparate)
    {
        List<string> separated = new List<string>();

        string currentString = "";
        for (int c = 0; c < stringToSeparate.Length; c++)
        {

            if (stringToSeparate[c] == ',')
            {
                separated.Add(currentString);
                currentString = "";
            }
            else
            {
                currentString += stringToSeparate[c];
            }
        }

        return separated.ToArray();
    }
}
