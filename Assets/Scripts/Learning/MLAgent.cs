using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MLAgent : MonoBehaviour, IComparable<MLAgent>
{
    public float fitness = 0;

    private NeuralNetwork neuralNetwork;

    public event EventHandler AgentFinished;

    public int CompareTo(MLAgent other)
    {
        return fitness.CompareTo(other.fitness);
    }

    public void SetNetwork(NeuralNetwork network)
    {
        neuralNetwork = network;
    }

    public NeuralNetwork GetNetwork()
    {
        return neuralNetwork;
    }
}
