using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MLTrainer : MonoBehaviour
{
    [Header("ML Trainer")]
    [SerializeField] protected int[] neuralNetLayers;
    [SerializeField] protected int agentAmount = 2;
    [SerializeField] protected int maxGenerations = 1000;
    [SerializeField] protected float gameSpeed = 1f;

    [SerializeField] protected MLAgent agentPrefab;

    protected NeuralNetwork[] networks;
    protected List<MLAgent> agents;
    protected List<bool> agentFinished;

    protected int currentGeneration;

    protected virtual void Start()
    {
        Initialize();

        StartGeneration();
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

        Time.timeScale = gameSpeed;
        currentGeneration++;
        Debug.Log("Starting generation: " + currentGeneration);

        InitAgents();
    }

    protected virtual void InitAgents()
    {
        agents = new List<MLAgent>();
        agentFinished = new List<bool>();

        for (int i = 0; i < networks.Length; i++)
        {
            agents.Add(SpawnAgent(networks[i], i));
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
        Debug.Log("Hightest of generation: " + agents[0].fitness);

        KillAgents();

        ModifyNetworks();

        StartGeneration();
    }

    protected virtual void KillAgents()
    {
        foreach (MLAgent agent in agents)
            Destroy(agent);
        agents.Clear();
    }

    protected virtual void ModifyNetworks()
    {

    }

    protected virtual MLAgent SpawnAgent(NeuralNetwork network, int agentNum)
    {
        MLAgent newAgent = Instantiate(agentPrefab, transform);
        newAgent.SetNetwork(network);
        newAgent.AgentFinished += HandleAgentFinished;
        return newAgent;
    }

    protected virtual void HandleAgentFinished(object sender, float e)
    {
        int agentIndex = agents.IndexOf((MLAgent)sender);

        agentFinished[agentIndex] = true;

        if (!agentFinished.Contains(false))
            EndGeneration();
    }
}
