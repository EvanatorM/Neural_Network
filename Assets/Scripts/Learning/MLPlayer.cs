using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MLPlayer : MonoBehaviour
{
    [Header("ML Player")]

    [SerializeField] protected MLAgent agentPrefab;

    [SerializeField] protected string networkSaveLocation;

    protected NeuralNetwork network;
    protected MLAgent agent;

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

    protected virtual MLAgent SpawnAgent(NeuralNetwork network)
    {
        MLAgent newAgent = Instantiate(agentPrefab, transform);
        newAgent.SetNetwork(network);
        newAgent.AgentFinished += HandleAgentFinished;
        return newAgent;
    }

    protected virtual void HandleAgentFinished(object sender, float e)
    {
        KillAgent();
    }
}
