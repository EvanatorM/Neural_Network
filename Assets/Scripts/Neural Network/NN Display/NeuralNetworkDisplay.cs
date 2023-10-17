using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NeuralNetworkDisplay : MonoBehaviour
{
    struct NeuronDisplay
    {
        public NeuronDisplayObject neuronObject;
        public NeuralNetwork.Neuron neuron;

        public NeuronDisplay(NeuronDisplayObject neuronObject, NeuralNetwork.Neuron neuron)
        {
            this.neuronObject = neuronObject;
            this.neuron = neuron;
        }
    }

    [Header("Neural Network")]
    [SerializeField] int[] layers;

    [Header("Display")]
    [SerializeField] NeuronDisplayObject neuronDisplayPrefab;
    [SerializeField] GameObject layerDisplayPrefab;
    [SerializeField] ConnectorDisplay connectorPrefab;
    [SerializeField] GameObject connectorParent;
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

        double[] inputs = new double[layers[0]];
        for (int i = 0; i < inputs.Length; i++)
        {
            inputs[i] = 0.5;
        }
        double[] values = neuralNet.RunNeuralNetwork(inputs);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            double[] inputs = new double[layers[0]];
            for (int i = 0; i < inputs.Length; i++)
            {
                inputs[i] = 0.5;
            }

            double[] values = neuralNet.RunNeuralNetwork(inputs);
            string valueText = "";
            for (int i = 0; i < values.Length; i++)
            {
                valueText += values[i] + " ";
            }    
            Debug.Log(valueText);
        }
    }

    void DisplayNeuralNet()
    {
        
        int numLayers = neuralNet.GetNumLayers();
        
        // Generate neurons
        List<List<NeuronDisplay>> layers = new List<List<NeuronDisplay>>();
        for (int i = 0; i < numLayers; i++)
        {
            NeuralNetwork.Neuron[] neurons = neuralNet.GetLayer(i);
            List<NeuronDisplay> currentLayer = GenerateNeuronsInLayer(neurons, i == 0);
            layers.Add(currentLayer);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(layerParent.GetComponent<RectTransform>());

        // Generate connectors
        for (int i = 0; i < numLayers - 1; i++)
        {
            for (int c = 0; c < layers[i].Count; c++)
            {
                for (int n = 0; n < layers[i + 1].Count; n++)
                {
                    // Calculate connector position
                    Vector2 oldPos = layers[i][c].neuronObject.transform.position;
                    Vector2 newPos = layers[i + 1][n].neuronObject.transform.position;
                    Vector2 connectorPosition = (oldPos + newPos) * .5f;

                    // Calculate connector rotation
                    Vector3 direction = newPos - oldPos;
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                    // Calculate connector size
                    float distance = direction.magnitude;
                    Vector2 connectorSize = new Vector2(distance * 1.36f, 10f);

                    ConnectorDisplay newConnector = Instantiate(connectorPrefab, connectorPosition, Quaternion.Euler(0f, 0f, angle), connectorParent.transform);
                    newConnector.InitConnector(layers[i + 1][n].neuron.neuronConnections[c]);
                    newConnector.GetComponent<RectTransform>().sizeDelta = connectorSize;

                    loadedConnectors.Add(newConnector.gameObject);
                }
            }
        }
    }

    List<NeuronDisplay> GenerateNeuronsInLayer(NeuralNetwork.Neuron[] layer, bool inputLayer)
    {
        List<NeuronDisplay> newNeurons = new List<NeuronDisplay>();

        GameObject newLayer = Instantiate(layerDisplayPrefab, layerParent);
        loadedLayerDisplays.Add(newLayer);

        for (int n = 0; n < layer.Length; n++)
        {
            NeuronDisplayObject newNeuron = Instantiate(neuronDisplayPrefab, newLayer.transform);
            newNeuron.InitNeuron(layer[n], this, inputLayer);
            NeuronDisplay newNeuronDisplay = new NeuronDisplay(newNeuron, layer[n]);
            loadedNeuronDisplays.Add(newNeuronDisplay);
            newNeurons.Add(newNeuronDisplay);
        }

        return newNeurons;
    }

    public void RunNeuralNet()
    {
        NeuralNetwork.Neuron[] inputLayer = neuralNet.GetLayer(0);
        double[] inputs = new double[inputLayer.Length];
        for (int i = 0; i < inputs.Length; i++)
        {
            inputs[i] = inputLayer[i].value;
        }

        neuralNet.RunNeuralNetwork(inputs);
    }
}
