using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NeuronDisplayObject : MonoBehaviour
{
    [SerializeField] TMP_Text valueText;
    [SerializeField] Image fillImage;
    [SerializeField] GameObject sliderObject;

    NeuralNetwork.Neuron neuron;
    NeuralNetworkDisplay nndisplay;
    bool input;

    public void InitNeuron(NeuralNetwork.Neuron neuron, NeuralNetworkDisplay nndisplay, bool input)
    {
        this.neuron = neuron;
        this.nndisplay = nndisplay;
        this.input = input;

        sliderObject.SetActive(input);
    }

    void Update()
    {
        if (neuron == null)
            return;

        valueText.text = neuron.value.ToString("0.00");
        fillImage.fillAmount = (float)neuron.value;
    }

    public void SliderValueChanged(float value)
    {
        if (!input)
            return;

        neuron.value = value;
        nndisplay.RunNeuralNet();
    }
}
