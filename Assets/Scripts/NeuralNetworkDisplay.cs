using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NeuralNetworkDisplay : MonoBehaviour
{
    struct NeuronDisplay
    {
        public GameObject neuronObject;
        public NeuralNetwork.Neuron neuron;

        public NeuronDisplay(GameObject neuronObject, NeuralNetwork.Neuron neuron)
        {
            this.neuronObject = neuronObject;
            this.neuron = neuron;
        }
    }

    [Header("Neural Network")]
    [SerializeField] int[] layers;

    [Header("Display")]
    [SerializeField] GameObject neuronDisplayPrefab;
    [SerializeField] GameObject layerDisplayPrefab;
    [SerializeField] GameObject connectorPrefab;
    [SerializeField] GameObject canvas;
    [SerializeField] Transform layerParent;
    [SerializeField] float minX, maxX, minY, maxY;

    NeuralNetwork neuralNet;

    List<GameObject> loadedLayerDisplays = new List<GameObject>();
    List<NeuronDisplay> loadedNeuronDisplays = new List<NeuronDisplay>();
    List<GameObject> loadedConnectors = new List<GameObject>();

    void Start()
    {
        neuralNet = new NeuralNetwork(layers);

        DisplayNeuralNet();
    }

    void DisplayNeuralNet()
    {
        
        int numLayers = neuralNet.GetNumLayers();
        /*
        // Generate Neurons
        for (int i = 0; i < numLayers; i++)
        {
            NeuralNetwork.Neuron[] neurons = neuralNet.GetLayer(i);
            GameObject newLayer = Instantiate(layerDisplayPrefab, layerParent);
            loadedLayerDisplays.Add(newLayer);

            for (int n = 0; n < neurons.Length; n++)
            {
                GameObject newNeuron = Instantiate(neuronDisplayPrefab, newLayer.transform);
                loadedNeuronDisplays.Add(new NeuronDisplay(newNeuron, neurons[n]));
            }    
        }*/

        // Generate First Layer
        NeuralNetwork.Neuron[] firstLayerNeurons = neuralNet.GetLayer(0);
        List<NeuronDisplay> currentLayer = GenerateNeuronsInLayer(firstLayerNeurons);

        for (int i = 0; i < numLayers - 1; i++)
        {
            // Generate next layer
            NeuralNetwork.Neuron[] nextLayerNeurons = neuralNet.GetLayer(i + 1);
            List<NeuronDisplay> nextLayer = GenerateNeuronsInLayer(nextLayerNeurons);

            LayoutRebuilder.ForceRebuildLayoutImmediate(layerParent.GetComponent<RectTransform>());

            // Create connectors between the current and new layer
            for (int c = 0; c < currentLayer.Count; c++)
            {
                for (int n = 0; n < nextLayer.Count; n++)
                {
                    // Calculate connector position
                    Vector2 oldPos = currentLayer[c].neuronObject.transform.position;
                    Vector2 newPos = nextLayer[n].neuronObject.transform.position;
                    Vector2 connectorPosition = (oldPos + newPos) * .5f;
                    Debug.Log(oldPos + ", " + newPos);

                    // Calculate connector rotation
                    Vector3 direction = newPos - oldPos;
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                    // Calculate connector size
                    float distance = direction.magnitude;
                    Vector2 connectorSize = new Vector2(distance * 1.36f, 10f);

                    GameObject newConnector = Instantiate(connectorPrefab, connectorPosition, Quaternion.Euler(0f, 0f, angle), canvas.transform);
                    newConnector.GetComponent<RectTransform>().sizeDelta = connectorSize;

                    loadedConnectors.Add(newConnector);
                }
            }
        }

        // Generate 
        // Go through layers
        // Generate current layer and save to list
        // Generate next layer if there is a next layer and save to list
        // Create connectors between the two layers
        // Set current layer list to next layer list
    }

    List<NeuronDisplay> GenerateNeuronsInLayer(NeuralNetwork.Neuron[] layer)
    {
        List<NeuronDisplay> newNeurons = new List<NeuronDisplay>();

        GameObject newLayer = Instantiate(layerDisplayPrefab, layerParent);
        loadedLayerDisplays.Add(newLayer);

        for (int n = 0; n < layer.Length; n++)
        {
            GameObject newNeuron = Instantiate(neuronDisplayPrefab, newLayer.transform);
            NeuronDisplay newNeuronDisplay = new NeuronDisplay(newNeuron, layer[n]);
            loadedNeuronDisplays.Add(newNeuronDisplay);
            newNeurons.Add(newNeuronDisplay);
        }

        return newNeurons;
    }
}
