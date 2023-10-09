using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNetworkDisplay : MonoBehaviour
{
    [Header("Neural Network")]
    [SerializeField] int[] layers;

    [Header("Display")]
    [SerializeField] GameObject neuronDisplayPrefab;
    [SerializeField] GameObject layerDisplayPrefab;
    [SerializeField] Transform layerParent;
    [SerializeField] float minX, maxX, minY, maxY;

    NeuralNetwork neuralNet;

    List<GameObject> loadedLayerDisplays = new List<GameObject>();
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
            GameObject newLayer = Instantiate(layerDisplayPrefab, layerParent);
            loadedLayerDisplays.Add(newLayer);

            for (int n = 0; n < neuron.Length; n++)
            {
                GameObject newNeuron = Instantiate(neuronDisplayPrefab, newLayer.transform);
                loadedNeuronDisplays.Add(newNeuron);
            }    
        }
    }
}
