using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NNPlayer : MonoBehaviour
{
    [Header("NN Player")]

    [SerializeField] protected NNAgent agentPrefab;

    [SerializeField] protected string networkSaveLocation;

    protected NeuralNetwork network;
    protected NNAgent agent;

    protected int currentGeneration;

    protected virtual void Start()
    {
        Initialize();

        StartPlayer();
    }

    protected virtual void Initialize()
    {
        string[] netString = File.ReadAllLines(networkSaveLocation);
        network = new NeuralNetwork(netString);
    }

    protected virtual void StartPlayer()
    {
        agent = SpawnAgent(network);
    }

    protected virtual void KillAgent()
    {
        Destroy(agent);
    }

    protected virtual NNAgent SpawnAgent(NeuralNetwork network)
    {
        NNAgent newAgent = Instantiate(agentPrefab, transform);
        newAgent.SetNetwork(network);
        newAgent.AgentFinished += HandleAgentFinished;
        return newAgent;
    }

    protected virtual void HandleAgentFinished(object sender, float e)
    {
        KillAgent();
    }
}
