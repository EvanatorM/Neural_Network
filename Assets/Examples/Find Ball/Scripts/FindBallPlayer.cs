using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindBallPlayer : MLPlayer
{
    [SerializeField] FindBallEnvironment environment;

    List<FindBallEnvironment> env = new List<FindBallEnvironment>();

    protected override MLAgent SpawnAgent(NeuralNetwork network)
    {
        FindBallEnvironment newEnvironment = Instantiate(environment, transform);
        env.Add(newEnvironment);
        newEnvironment.StartEnvironment(network);
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
