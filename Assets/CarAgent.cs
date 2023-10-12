using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAgent : MLAgent
{
    [SerializeField] float speed = 10f;
    [SerializeField] float rotateSpeed = 10f;

    [SerializeField] int numRaycasts = 5;
    [SerializeField] float rayDistance = 5.0f;

    Rigidbody rb;

    CarTrainer carTrainer;

    bool init = false;
    float timeToLive;
    bool training;

    int currentGoal = 0;

    public void InitAgent(CarTrainer trainer, float timeToLive, bool training)
    {
        rb = GetComponent<Rigidbody>();

        carTrainer = trainer;

        this.timeToLive = timeToLive;
        this.training = training;

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
        double[] inputs = new double[numRaycasts];
        float[] distances = GetRaycasts();
        for (int i = 0; i < inputs.Length; i++)
        {
            inputs[i] = distances[i];
        }

        double[] outputs = neuralNetwork.RunNeuralNetwork(inputs);

        transform.Rotate(Vector3.up, (float)outputs[1] * rotateSpeed * Time.deltaTime);
        rb.velocity = (float)outputs[0] * speed * -transform.right;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
            FinishAgent();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Goal goal))
        {
            if (carTrainer.IsNextGoal(goal.gameObject, currentGoal))
                fitness++;
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
                distances[i] = 6;
        }

        return distances;
    }

    void OnDrawGizmos()
    {
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
