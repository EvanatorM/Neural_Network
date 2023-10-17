using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindBallPlayer : NNPlayer
{
    [SerializeField] FindBallEnvironment environment;
    [SerializeField] bool resetPlayerPos;

    List<FindBallEnvironment> env = new List<FindBallEnvironment>();

    protected override NNAgent SpawnAgent(NeuralNetwork network)
    {
        FindBallEnvironment newEnvironment = Instantiate(environment, transform);
        env.Add(newEnvironment);
        newEnvironment.StartEnvironment(network, resetPlayerPos, false);
        newEnvironment.agent.AgentFinished += HandleAgentFinished;

        return newEnvironment.agent;
    }

    protected override void KillAgent()
    {
        foreach (FindBallEnvironment e in env)
            Destroy(e.gameObject);
        env.Clear();
    }
}
