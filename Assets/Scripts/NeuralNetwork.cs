using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNetwork
{
    public class Neuron
    {

    }

    public class NeuronConnection
    {

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
