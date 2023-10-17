using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAgent : NNAgent
{
    [SerializeField] float speed = 10f;
    [SerializeField] float acceleration = 10f;
    [SerializeField] float rotateSpeed = 10f;

    [SerializeField] int numRaycasts = 5;
    [SerializeField] float rayDistance = 5.0f;

    Rigidbody rb;

    CarTrainer carTrainer;
    CarPlayer carPlayer;

    bool init = false;
    float timeToLive;
    bool training;
    int numGoals;
    bool accelerationMode;

    int currentGoal = 0;

    public void InitAgent(CarTrainer trainer, float timeToLive, bool training, int numGoals, bool accelerationMode)
    {
        rb = GetComponent<Rigidbody>();

        carTrainer = trainer;

        this.timeToLive = timeToLive;
        this.training = training;
        this.numGoals = numGoals;
        this.accelerationMode = accelerationMode;
        if (accelerationMode)
            speed = 0;

        init = true;
    }

    public void InitAgent(CarPlayer player, float timeToLive, bool training, int numGoals, bool accelerationMode)
    {
        rb = GetComponent<Rigidbody>();

        carPlayer = player;

        this.timeToLive = timeToLive;
        this.training = training;
        this.numGoals = numGoals;
        this.accelerationMode = accelerationMode;
        if (accelerationMode)
            speed = 0;

        init = true;
    }

    void Update()
    {

        if (!init)
            return;

        if (training)
        {
            timeToLive -= Time.deltaTime;
            if (timeToLive <= 0)
                FinishAgent();
        }
    }

    private void FixedUpdate()
    {
        if (!init)
            return;

        // Get inputs from neural network
        double[] inputs;
        if (accelerationMode)
            inputs = new double[numRaycasts + 1];
        else
            inputs = new double[numRaycasts];
        float[] distances = GetRaycasts();
        for (int i = 0; i < numRaycasts; i++)
        {
            inputs[i] = distances[i];
        }

        if (accelerationMode)
            inputs[numRaycasts] = speed;

        double[] outputs = neuralNetwork.RunNeuralNetwork(inputs);

        transform.Rotate(Vector3.up, (float)outputs[1] * rotateSpeed * Time.deltaTime);

        if (accelerationMode)
        {
            speed += (float)outputs[0] * acceleration * Time.deltaTime;
            rb.velocity = speed * -transform.right;
        }   
        else
            rb.velocity = (float)outputs[0] * speed * -transform.right;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!init)
            return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            FinishAgent();
            rb.isKinematic = true;
            GetComponent<Collider>().enabled = false;
            init = false;
            rb.velocity = Vector3.zero;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!init)
            return;

        if (other.TryGetComponent(out Goal goal))
        {
            bool isNextGoal = carTrainer == null ? carPlayer.IsNextGoal(goal.gameObject, currentGoal) : carTrainer.IsNextGoal(goal.gameObject, currentGoal);
            if (isNextGoal)
            {
                fitness++;
                currentGoal++;
                currentGoal %= numGoals;
            }
        }
    }

    float[] GetRaycasts()
    {
        float[] distances = new float[numRaycasts];
        for (int i = 0; i < numRaycasts; i++)
        {
            float angle_i = (i / (float)(numRaycasts - 1)) * Mathf.PI;
            Vector3 direction = Quaternion.Euler(0, angle_i * Mathf.Rad2Deg, 0) * transform.forward;

            Ray ray = new Ray(transform.position, direction);
            Physics.Raycast(ray, out RaycastHit hit, rayDistance, 1 << 7);
            distances[i] = hit.distance;
            if (hit.collider == null)
                distances[i] = rayDistance + 1;
        }

        return distances;
    }

    void OnDrawGizmos()
    {
        if (!init)
            return;

        float[] distances = GetRaycasts();
        for (int i = 0; i < numRaycasts; i++)
        {
            float angle_i = (i / (float)(numRaycasts - 1)) * Mathf.PI;
            Vector3 direction = Quaternion.Euler(0, angle_i * Mathf.Rad2Deg, 0) * transform.forward;

            Gizmos.color = new Color(Mathf.Lerp(1f, 0f, distances[i] / rayDistance), 0f, 0f);
            Gizmos.DrawRay(transform.position, direction * rayDistance);
        }
    }
}
