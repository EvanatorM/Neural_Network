using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicTacToeTrainer : NNGeneticAlgTrainer
{
    [SerializeField] TicTacToe tttPrefab;
    [SerializeField] int roundsPerGeneration;
    [SerializeField] bool playerTrained;

    List<TicTacToe> activeGames = new List<TicTacToe>();

    protected override void InitAgents()
    {
        StartCoroutine(DelayInitAgents());
    }

    // To avoid stack overflow errors
    IEnumerator DelayInitAgents()
    {
        yield return null;

        agents = new List<NNAgent>();
        agentFinished = new List<bool>();

        bool player1First = Random.Range(0, 2) == 0 ? true : false;

        for (int i = 0; i < networks.Length; i += 2)
        {
            TicTacToe newGame = Instantiate(tttPrefab, transform);

            TicTacToeAgent agent1 = null;
            if (!player1First || !playerTrained)
            {
                agent1 = (TicTacToeAgent)Instantiate(agentPrefab, newGame.transform);
                agent1.SetNetwork(networks[i]);
                agent1.AgentFinished += HandleAgentFinished;
                agents.Add(agent1);
                agentFinished.Add(false);
            }

            TicTacToeAgent agent2 = null;
            if (player1First || !playerTrained)
            {
                agent2 = (TicTacToeAgent)Instantiate(agentPrefab, newGame.transform);
                agent2.SetNetwork(networks[i + 1]);
                agent2.AgentFinished += HandleAgentFinished;
                agents.Add(agent2);
                agentFinished.Add(false);
            }

            activeGames.Add(newGame);
            yield return null;
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
