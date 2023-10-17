using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NNGeneticAlgTrainer : NNTrainer
{
    [Header("Genetic Algorithm")]
    [SerializeField, Range(0f, 1f)] protected float mutateRate;
    [SerializeField, Range(0f, 1f)] protected double weightMutationStrength;
    [SerializeField, Range(0f, 10f)] protected double biasMutationStrength;

    protected override void ModifyNetworks()
    {
        //string[] output = networks[0].OutputNetwork(bestFitness);
        //foreach (string s in output)
        //    Debug.Log(s);

        int halfNetworks = networks.Length / 2;

        for (int i = 0; i < halfNetworks; i++)
        {
            networks[i + halfNetworks] = new NeuralNetwork(networks[i]);
            networks[i + halfNetworks].Mutate(mutateRate, weightMutationStrength, biasMutationStrength);
        }
    }
}
