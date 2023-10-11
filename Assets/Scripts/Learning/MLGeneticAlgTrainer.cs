using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MLGeneticAlgTrainer : MLTrainer
{
    [Header("Genetic Algorithm")]
    [SerializeField, Range(0f, 1f)] protected float mutateRate;
    [SerializeField, Range(0f, 1f)] protected double weightMutationStrength;
    [SerializeField, Range(0f, 10f)] protected double biasMutationStrength;

    protected override void ModifyNetworks()
    {
        int halfNetworks = networks.Length / 2;

        for (int i = 0; i < halfNetworks; i++)
        {
            networks[i + halfNetworks] = new NeuralNetwork(networks[i]);
            networks[i + halfNetworks].Mutate(mutateRate, weightMutationStrength, biasMutationStrength);
        }
    }
}
