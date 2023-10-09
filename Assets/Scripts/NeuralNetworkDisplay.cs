using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNetworkDisplay : MonoBehaviour
{
    [Header("Neural Network")]
    [SerializeField] int[] layers;

    [Header("Display")]
    [SerializeField] GameObject neuronDisplay;
    [SerializeField] float minX, maxX, minY, maxY;

    NeuralNetwork neuralNet;

    List<GameObject> loadedNeuronDisplays = new List<GameObject>();

    void Start()
    {
        neuralNet = new NeuralNetwork(layers);

        DisplayNeuralNet();
    }

    void DisplayNeuralNet()
    {
        int numLayers = neuralNet.GetNumLayers();

        for (int i = 0; i < numLayers; i++)
        {
            NeuralNetwork.Neuron[] neuron = neuralNet.GetLayer(i);
            for (int n = 0; n < neuron.Length; n++)
            {
                Vector2 neuronPos = new Vector2(Mathf.Lerp(minX, maxX, i / (float)numLayers), Mathf.Lerp(minY, maxY, n / (float)neuron.Length));
                GameObject newNeuron = Instantiate(neuronDisplay, neuronPos, Quaternion.identity, transform);
                loadedNeuronDisplays.Add(newNeuron);
            }    
        }
    }
}
