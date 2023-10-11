using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindBallTrainer : MLGeneticAlgTrainer
{
    [SerializeField] FindBallEnvironment environment;
    [SerializeField] bool resetPlayerPos;

    List<FindBallEnvironment> env = new List<FindBallEnvironment>();

    protected override MLAgent SpawnAgent(NeuralNetwork network, int agentNum)
    {
        FindBallEnvironment newEnvironment = Instantiate(environment, transform);
        env.Add(newEnvironment);
        newEnvironment.StartEnvironment(network, resetPlayerPos);
        newEnvironment.agent.AgentFinished += HandleAgentFinished;

        return newEnvironment.agent;
    }

    protected override void KillAgents()
    {
        foreach (FindBallEnvironment e in env)
            Destroy(e.gameObject);
        env.Clear();
    }
}
