using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPlayer : MLPlayer
{
    [SerializeField] Transform startPos;
    [SerializeField] GameObject[] goals;

    public bool IsNextGoal(GameObject goal, int currentGoal)
    {
        for (int i = 0; i < goals.Length; i++)
        {
            if (goals[i] == goal)
            {
                return i == currentGoal;
            }
        }

        return false;
    }

    protected override MLAgent SpawnAgent(NeuralNetwork network)
    {
        MLAgent newAgent = Instantiate(agentPrefab, startPos.position, startPos.rotation, transform);
        newAgent.SetNetwork(network);
        ((CarAgent)newAgent).InitAgent(this, 0, false, goals.Length);
        newAgent.AgentFinished += HandleAgentFinished;
        return newAgent;
    }
}
