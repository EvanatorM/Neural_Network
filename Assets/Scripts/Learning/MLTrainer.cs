using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MLTrainer : MonoBehaviour
{
    [SerializeField] protected int[] neuralNetLayers;
    [SerializeField] protected int agentAmount = 2;
    [SerializeField] protected int maxGenerations = 1000;

    [SerializeField] protected MLAgent agentPrefab;

    protected NeuralNetwork[] networks;
    protected List<MLAgent> agents;
    protected List<bool> agentFinished;

    protected int currentGeneration;

    protected virtual void Start()
    {
        Initialize();
        StartGeneration();
        EndGeneration();
    }

    protected virtual void Initialize()
    {
        networks = new NeuralNetwork[agentAmount];
        for (int i = 0; i < agentAmount; i++)
        {
            networks[i] = new NeuralNetwork(neuralNetLayers);
        }
    }

    protected virtual void StartGeneration()
    {
        if (currentGeneration > maxGenerations)
            return;

        currentGeneration++;

        InitAgents();
    }

    protected virtual void InitAgents()
    {
        agents = new List<MLAgent>();
        agentFinished = new List<bool>();

        for (int i = 0; i < networks.Length; i++)
        {
            agents.Add(SpawnAgent(networks[i]));
            agentFinished.Add(false);
        }
    }

    protected virtual void EndGeneration()
    {
        agents.Sort();

        for (int i = 0; i < agents.Count; i++)
        {
            networks[i] = new NeuralNetwork(agents[i].GetNetwork());
        }

        foreach (MLAgent agent in agents)
            Destroy(agent);
        agents.Clear();

        StartGeneration();
    }

    protected virtual MLAgent SpawnAgent(NeuralNetwork network)
    {
        MLAgent newAgent = Instantiate(agentPrefab, transform);
        newAgent.SetNetwork(network);
        newAgent.AgentFinished += HandleAgentFinished;
        return newAgent;
    }

    protected virtual void HandleAgentFinished(object sender, EventArgs e)
    {
        int agentIndex = agents.IndexOf((MLAgent)sender);

        agentFinished[agentIndex] = true;

        if (!agentFinished.Contains(false))
            EndGeneration();
    }
}
