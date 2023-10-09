using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConnectorDisplay : MonoBehaviour
{
    [SerializeField] Image fillImage;

    NeuralNetwork.NeuronConnection connection;

    public void InitConnector(NeuralNetwork.NeuronConnection connection)
    {
        this.connection = connection;
    }

    void Update()
    {
        if (connection == null)
            return;

        Color baseColor = connection.weight > 0 ? new Color(0.6f, 1f, 0.6f) : new Color(1f, 0.6f, 0.6f);
        fillImage.color = Color.Lerp(Color.black, baseColor, Mathf.Abs((float)connection.weight));
    }
}
