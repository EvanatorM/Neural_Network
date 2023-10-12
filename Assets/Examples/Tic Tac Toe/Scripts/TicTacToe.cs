using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TicTacToe : MonoBehaviour
{
    [SerializeField] int[] board;
    [SerializeField] Image[] boardUI;
    [SerializeField] Sprite xSprite, oSprite;

    [SerializeField] bool player1Player, player2Player;

    TicTacToeAgent agent1, agent2;
    int rounds;

    bool player1Turn = true;

    private void Start()
    {
        InitTicTacToe(null, null, 100);
    }

    public void InitTicTacToe(TicTacToeAgent agent1, TicTacToeAgent agent2, int rounds)
    {
        if (agent1 == null)
            player1Player = true;
        if (agent2 == null)
            player2Player = true;
        this.agent1 = agent1;
        this.agent2 = agent2;
        this.rounds = rounds;

        board = new int[9];
        player1Turn = true;

        DisplayBoard();

        StartNextTurn();
    }

    void RestartTicTacToe()
    {
        rounds--;
        if (rounds <= 0)
        {
            if (!player1Player)
                agent1.FinishTraining();
            if (!player2Player)
                agent2.FinishTraining();
            return;
        }

        board = new int[9];
        player1Turn = true;

        DisplayBoard();

        StartNextTurn();
    }

    void StartNextTurn()
    {
        int[] mask = new int[9];
        for (int i = 0; i < mask.Length; i++)
        {
            if (board[i] == 0)
                mask[i] = 1;
            else
                mask[i] = 0;
        }

        if (player1Turn)
        {
            if (!player1Player)
                agent1.StartTurn(board, mask);
        }
        else
        {
            if (!player2Player)
                agent2.StartTurn(board, mask);
        }
    }

    public void TakeTurn(int space)
    {
        // Check if valid move
        if (board[space] != 0)
            return;

        // Make move
        board[space] = player1Turn ? 1 : 2;

        DisplayBoard();

        // Check if won
        int results = GetGameResults();
        if (results == -1) // Nobody won yet
        {
            player1Turn = !player1Turn;
            StartNextTurn();
        }
        else if (results == 0) // Tie
        {
            RestartTicTacToe();
        }
        else if (results == 1) // Player 1 Wins
        {
            if (!player1Player)
                agent1.fitness += 1;
            if (!player2Player)
                agent2.fitness -= 1;
            RestartTicTacToe();
        }
        else if (results == 2) // Player 2 Wins
        {
            if (!player1Player)
                agent1.fitness -= 1;
            if (!player2Player)
                agent2.fitness += 1;
            RestartTicTacToe();
        }
    }

    void DisplayBoard()
    {
        for (int i = 0; i < boardUI.Length; i++)
        {
            switch (board[i])
            {
                case 0:
                    boardUI[i].color = new Color(0, 0, 0, 0);
                    break;
                case 1:
                    boardUI[i].color = new Color(1, 1, 1, 1);
                    boardUI[i].sprite = xSprite;
                    break;
                case 2:
                    boardUI[i].color = new Color(1, 1, 1, 1);
                    boardUI[i].sprite = oSprite;
                    break;
            }
        }
    }

    int GetGameResults()
    {
        int p1Result = ResultPlayer(1);
        if (p1Result != -1)
            return p1Result;

        int p2Result = ResultPlayer(2);
        if (p2Result != -1)
            return p2Result;

        bool contains0 = false;
        for (int i = 0; i < board.Length; i++)
        {
            if (board[i] == 0)
            {
                contains0 = true;
                break;
            }
        }

        if (!contains0)
            return 0;

        return -1;
    }

    int ResultPlayer(int player)
    {
        if (board[0] == player && board[1] == player && board[2] == player)
            return player;
        if (board[3] == player && board[4] == player && board[5] == player)
            return player;
        if (board[6] == player && board[7] == player && board[8] == player)
            return player;
        if (board[0] == player && board[3] == player && board[6] == player)
            return player;
        if (board[1] == player && board[4] == player && board[7] == player)
            return player;
        if (board[2] == player && board[5] == player && board[8] == player)
            return player;
        if (board[0] == player && board[4] == player && board[8] == player)
            return player;
        if (board[2] == player && board[4] == player && board[6] == player)
            return player;

        return -1;
    }

    public void HandleBoardSpaceClicked(int space)
    {
        if (player1Turn)
        {
            if (player1Player)
                TakeTurn(space);
        }
        else
        {
            if (player2Player)
                TakeTurn(space);
        }
    }
}
