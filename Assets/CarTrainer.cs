using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarTrainer : MLGeneticAlgTrainer
{
    [SerializeField] Transform startPos;
    [SerializeField] GameObject[] goals;

    [SerializeField] float timeToLive = 10f;

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

    protected override MLAgent SpawnAgent(NeuralNetwork network, int agentNum)
    {
        MLAgent newAgent = Instantiate(agentPrefab, startPos.position, startPos.rotation, transform);
        newAgent.SetNetwork(network);
        ((CarAgent)newAgent).InitAgent(this, timeToLive, true);
        newAgent.AgentFinished += HandleAgentFinished;
        return newAgent;
    }
}
