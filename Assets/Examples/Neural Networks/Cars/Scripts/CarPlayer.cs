using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPlayer : NNPlayer
{
    [SerializeField] Transform startPos;
    [SerializeField] GameObject[] goals;

    [SerializeField] bool accelerateMode;

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

    protected override NNAgent SpawnAgent(NeuralNetwork network)
    {
        NNAgent newAgent = Instantiate(agentPrefab, startPos.position, startPos.rotation, transform);
        newAgent.SetNetwork(network);
        ((CarAgent)newAgent).InitAgent(this, 0, false, goals.Length, accelerateMode);
        newAgent.AgentFinished += HandleAgentFinished;
        return newAgent;
    }
}
