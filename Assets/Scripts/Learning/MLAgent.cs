using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MLAgent : MonoBehaviour, IComparable<MLAgent>
{
    public float fitness = 0;

    protected NeuralNetwork neuralNetwork;

    public event EventHandler<float> AgentFinished;

    public void SetNetwork(NeuralNetwork network)
    {
        neuralNetwork = network;
    }

    public virtual void FinishAgent()
    {
        AgentFinished?.Invoke(this, fitness);
    }

    public NeuralNetwork GetNetwork()
    {
        return neuralNetwork;
    }

    public int CompareTo(MLAgent other)
    {
        if (fitness > other.fitness)
            return -1;
        else if (fitness < other.fitness)
            return 1;
        else
            return 0;
    }
}
