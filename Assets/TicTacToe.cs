using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TicTacToe : MonoBehaviour
{
    [SerializeField] int[] board;
    [SerializeField] Image[] boardUI;
    [SerializeField] Sprite xSprite, oSprite;

    TicTacToeAgent agent1, agent2;

    bool player1Turn = true;

    void Start()
    {
        DisplayBoard();
    }

    public void InitTicTacToe(TicTacToeAgent agent1, TicTacToeAgent agent2)
    {
        this.agent1 = agent1;
        this.agent2 = agent2;
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

    public void HandleBoardSpaceClicked(int space)
    {
        board[space] = player1Turn ? 1 : 2;
        player1Turn = !player1Turn;

        DisplayBoard();
    }
}
