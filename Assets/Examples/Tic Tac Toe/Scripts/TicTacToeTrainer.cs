using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicTacToeTrainer : MLGeneticAlgTrainer
{
    [SerializeField] TicTacToe tttPrefab;
    [SerializeField] int roundsPerGeneration;

    List<TicTacToe> activeGames = new List<TicTacToe>();

    protected override void InitAgents()
    {
        StartCoroutine(DelayInitAgents());
    }

    // To avoid stack overflow errors
    IEnumerator DelayInitAgents()
    {
        yield return null;

        agents = new List<MLAgent>();
        agentFinished = new List<bool>();

        for (int i = 0; i < networks.Length; i += 2)
        {
            TicTacToe newGame = Instantiate(tttPrefab, transform);
            TicTacToeAgent agent1 = (TicTacToeAgent)Instantiate(agentPrefab, newGame.transform);
            agent1.SetNetwork(networks[i]);
            agent1.AgentFinished += HandleAgentFinished;
            TicTacToeAgent agent2 = (TicTacToeAgent)Instantiate(agentPrefab, newGame.transform);
            agent2.SetNetwork(networks[i + 1]);
            agent2.AgentFinished += HandleAgentFinished;
            agents.Add(agent1);
            agents.Add(agent2);
            agentFinished.Add(false);
            agentFinished.Add(false);

            activeGames.Add(newGame);
            newGame.InitTicTacToe(agent1, agent2, roundsPerGeneration);
        }
    }

    protected override void KillAgents()
    {
        base.KillAgents();

        foreach (TicTacToe t in activeGames)
            Destroy(t.gameObject);
        activeGames.Clear();
    }
}
