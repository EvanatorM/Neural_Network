using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNetwork
{
    public class Neuron
    {
        public NeuronConnection[] neuronConnections;
        public double value;
        public double bias;

        public Neuron()
        {
            bias = Random.Range(-8f, 8f);
        }

        public Neuron(Neuron neuronToCopy)
        {
            bias = neuronToCopy.bias;
            value = neuronToCopy.value;
        }

        public void SetConnections(NeuronConnection[] neuronConnections)
        {
            this.neuronConnections = neuronConnections;
        }
    }

    public class NeuronConnection
    {
        public Neuron previousNeuron;
        public double weight;

        public NeuronConnection(Neuron previousNeuron)
        {
            this.previousNeuron = previousNeuron;
            weight = Random.Range(-1f, 1f);
        }

        public NeuronConnection(Neuron previousNeuron, NeuronConnection connectionToCopy)
        {
            this.previousNeuron = previousNeuron;
            weight = connectionToCopy.weight;
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

                if (i == 0)
                    continue;

                // Create neuron connections
                NeuronConnection[] connections = new NeuronConnection[layers[i - 1]];
                for (int p = 0; p < layers[i - 1]; p++)
                {
                    connections[p] = new NeuronConnection(neurons[i - 1][p]);
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
                double value = 1 / (1 + System.Math.Pow(System.Math.E, -sum));

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
}
