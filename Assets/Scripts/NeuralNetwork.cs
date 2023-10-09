using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNetwork
{
    public class Neuron
    {
        public NeuronConnection[] neuronConnections;

        public Neuron(NeuronConnection[] neuronConnections)
        {
            this.neuronConnections = neuronConnections;
        }
    }

    public class NeuronConnection
    {
        public Neuron nextNeuron;

        public NeuronConnection(Neuron nextNeuron)
        {
            this.nextNeuron = nextNeuron;
        }
    }

    Neuron[][] neurons;

    public NeuralNetwork(int[] layers)
    {
        neurons = new Neuron[layers.Length][];

        for (int i = 0; i < layers.Length; i++)
        {
            neurons[i] = new Neuron[layers[i]];
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
}
