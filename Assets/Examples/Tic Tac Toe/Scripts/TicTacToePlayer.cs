using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicTacToePlayer : MLPlayer
{
    [SerializeField] TicTacToe tttPrefab;

    List<TicTacToe> activeGames = new List<TicTacToe>();

    protected override MLAgent SpawnAgent(NeuralNetwork network)
    {
        TicTacToe newGame = Instantiate(tttPrefab, transform);
        TicTacToeAgent agent1 = (TicTacToeAgent)Instantiate(agentPrefab, newGame.transform);
        agent1.SetNetwork(network);
        agent1.AgentFinished += HandleAgentFinished;

        activeGames.Add(newGame);
        newGame.InitTicTacToe(null, agent1, int.MaxValue);

        return agent1;
    }
}
