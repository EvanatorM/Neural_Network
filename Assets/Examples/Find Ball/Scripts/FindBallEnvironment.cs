using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindBallEnvironment : MonoBehaviour
{
    [SerializeField] public FindBallAgent agent;
    [SerializeField] GameObject goal;

    [SerializeField] float timeToLive = 10f;

    bool resetPlayerPos = true;

    public void StartEnvironment(NeuralNetwork network, bool resetPlayerPos)
    {
        this.resetPlayerPos = resetPlayerPos;

        agent.SetNetwork(network);
        agent.InitAgent(this, goal, timeToLive);
        RestartEnvironment();
    }

    public void RestartEnvironment()
    {
        if (resetPlayerPos)
            agent.transform.localPosition = Vector3.zero;

        do
        {
            goal.transform.localPosition = new Vector3(Random.Range(-4f, 4f), 0, Random.Range(-4f, 4f));
        }
        while (Vector3.Distance(agent.transform.position, goal.transform.position) < 3f);

        agent.ContinueAgent();
    }
}
