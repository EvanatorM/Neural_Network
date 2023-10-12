using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindBallAgent : MLAgent
{
    [SerializeField] float speed = 10f;

    Rigidbody rb;

    FindBallEnvironment environment;
    GameObject goal;
    bool init = false;

    float timeRemaining;
    bool training = true;

    public void InitAgent(FindBallEnvironment environment, GameObject goal, float timeRemaining, bool training)
    {
        rb = GetComponent<Rigidbody>();

        this.environment = environment;
        this.goal = goal;
        fitness = 0;

        this.timeRemaining = timeRemaining;
        this.training = training;
    }

    public void ContinueAgent()
    {
        init = true;
    }

    void Update()
    {
        if (!init)
            return;

        if (training)
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0)
                FinishAgent();
        }

        if (transform.position.y < -1)
        {
            fitness = -1;
            FinishAgent();
        }
    }

    private void FixedUpdate()
    {
        if (!init)
            return;

        // Get inputs from neural network
        double[] inputs = new double[4]
        {
            transform.localPosition.x,
            transform.localPosition.z,
            goal.transform.localPosition.x,
            goal.transform.localPosition.z
        };

        double[] outputs = neuralNetwork.RunNeuralNetwork(inputs);

        Vector3 vel = rb.velocity;
        vel.x = (float)outputs[0] * speed;
        vel.z = (float)outputs[1] * speed;

        rb.velocity = vel;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!init)
            return;

        if (other.gameObject == goal)
        {
            init = false;
            fitness++;
            environment.RestartEnvironment();
        }
    }
}
