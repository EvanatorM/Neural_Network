using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicTacToeAgent : NNAgent
{
    TicTacToe ttt;
    int playerNum;

    public void InitAgent(TicTacToe ttt, int playerNum)
    {
        this.ttt = ttt;
        this.playerNum = playerNum;
    }

    public void StartTurn(int[] board, int[] mask)
    {
        // 1-9: Board
        // 10: Player number
        double[] inputs = new double[10];

        for (int i = 0; i < board.Length; i++)
        {
            inputs[i] = board[i];
        }

        inputs[9] = playerNum;

        // Run neural network
        double[] outputs = neuralNetwork.RunNeuralNetwork(inputs);

        // Apply mask to outputs
        for (int i = 0; i < outputs.Length; i++)
        {
            if (mask[i] == 0)
                outputs[i] = -1;
        }

        // Get highest scoring space
        double maxScore = double.MinValue;
        int maxIndex = 0;
        for (int i = 0; i < outputs.Length; i++)
        {
            if (outputs[i] > maxScore)
            {
                maxScore = outputs[i];
                maxIndex = i;
            }
        }

        // Take turn with highest scoring space
        ttt.TakeTurn(maxIndex);
    }

    public void FinishTraining()
    {
        FinishAgent();
    }
}
