using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MLTrainer : MonoBehaviour
{
    [Header("ML Trainer")]
    [SerializeField] protected int[] neuralNetLayers;
    [SerializeField] protected int agentAmount = 2;
    [SerializeField] protected int maxGenerations = 1000;
    [SerializeField] protected float gameSpeed = 1f;

    [SerializeField] protected MLAgent agentPrefab;

    [SerializeField] protected string networkSaveLocation;

    protected NeuralNetwork[] networks;
    protected List<MLAgent> agents;
    protected List<bool> agentFinished;
    protected float bestFitness = 0;
    protected float bestFitnessThisGeneration = 0;

    protected int currentGeneration;

    public event EventHandler<int> GenerationStarted;
    public event EventHandler<float> HighestFitnessChanged;
    public event EventHandler<float> HighestFitnessThisGenChanged;

    protected virtual void Start()
    {
        Initialize();

        StartGeneration();
    }

    protected virtual void Initialize()
    {
        if (File.Exists(networkSaveLocation))
        {
            string[] netString = File.ReadAllLines(networkSaveLocation);
            bestFitness = NeuralNetwork.GetFitnessFromFile(netString);
            currentGeneration = NeuralNetwork.GetGenerationFromFile(netString);
            NeuralNetwork network = new NeuralNetwork(netString);

            networks = new NeuralNetwork[agentAmount];
            for (int i = 0; i < agentAmount; i++)
            {
                networks[i] = new NeuralNetwork(network);
            }
        }
        else
        {
            bestFitness = float.MinValue;
            networks = new NeuralNetwork[agentAmount];
            for (int i = 0; i < agentAmount; i++)
            {
                networks[i] = new NeuralNetwork(neuralNetLayers);
            }
        }
    }

    protected virtual void StartGeneration()
    {
        if (currentGeneration > maxGenerations)
            return;

        Time.timeScale = gameSpeed;
        currentGeneration++;
        GenerationStarted?.Invoke(this, currentGeneration);
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

        bestFitnessThisGeneration = agents[0].fitness;
        HighestFitnessThisGenChanged?.Invoke(this, bestFitnessThisGeneration);

        for (int i = 0; i < agents.Count; i++)
        {
            networks[i] = new NeuralNetwork(agents[i].GetNetwork());
        }
        Debug.Log("Hightest of generation: " + bestFitnessThisGeneration + ", Highest Overall: " + bestFitness);

        KillAgents();

        SaveNetwork();

        ModifyNetworks();

        if (bestFitnessThisGeneration > bestFitness)
        {
            bestFitness = bestFitnessThisGeneration;
            HighestFitnessChanged?.Invoke(this, bestFitness);
        }

        StartGeneration();
    }

    protected virtual void KillAgents()
    {
        foreach (MLAgent agent in agents)
            Destroy(agent.gameObject);
        agents.Clear();
        agentFinished.Clear();
    }

    protected virtual void ModifyNetworks()
    {
        
    }

    protected virtual void SaveNetwork()
    {
        if (bestFitnessThisGeneration > bestFitness)
        {
            Debug.Log("Saving new highest");
            string[] output = networks[0].OutputNetwork(bestFitnessThisGeneration, currentGeneration);
            File.WriteAllLines(networkSaveLocation, output);
        }
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
