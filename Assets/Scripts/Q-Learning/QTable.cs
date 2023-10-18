using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QActionSet
{
    public float[] actionQValues;

    public QActionSet(int numOutcomes)
    {
        actionQValues = new float[numOutcomes];
    }

    public int GetNumOutcomes()
    {
        return actionQValues.Length;
    }
}

public class QTable<E>
{
    public Dictionary<E, QActionSet> qTable;
    int numOutcomes;
    float learningRate;
    float discountFactor;
    float epsilon;

    public QTable(int numOutcomes)
    {
        this.numOutcomes = numOutcomes;
        learningRate = 0.1f;
        discountFactor = 0.9f;
        epsilon = 0.1f;
    }

    public QTable(int numOutcomes, float learningRate, float discountFactor, float epsilon)
    {
        this.numOutcomes = numOutcomes;
        this.learningRate = learningRate;
        this.discountFactor = discountFactor;
        this.epsilon = epsilon;
    }

    public int GetAction(E state)
    {
        if (!qTable.ContainsKey(state))
            qTable[state] = new QActionSet(numOutcomes);

        // Choose exploration or exploitation
        if (Random.value < epsilon)
        {
            // Exploration (Random action)
            return Random.Range(0, numOutcomes);
        }
        else
        {
            // Exploitation (Highest Q-value
            float[] qValues = qTable[state].actionQValues;
            int bestAction = 0;
            for (int i = 0; i < numOutcomes; i++)
            {
                if (qValues[i] > qValues[bestAction])
                    bestAction = i;
            }

            return bestAction;
        }
    }

    public void UpdateQValue(E state, int action, float reward, E nextState)
    {
        float stateQValue = qTable[state].actionQValues[action];
        float[] nextStateQValues = qTable[nextState].actionQValues;

        float maxNextQValue = float.MinValue;
        for (int i = 0; i < nextStateQValues.Length; i++)
        {
            if (nextStateQValues[i] > maxNextQValue)
                maxNextQValue = nextStateQValues[i];
        }

        qTable[state].actionQValues[action] = stateQValue + learningRate * (reward + discountFactor * maxNextQValue - stateQValue);
    }
}
