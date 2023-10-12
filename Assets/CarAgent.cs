using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAgent : MonoBehaviour
{
    [SerializeField] int numRaycasts = 5;
    [SerializeField] float rayDistance = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmos()
    {
        for (int i = 0; i < numRaycasts; i++)
        {
            float angle_i = (i / (float)(numRaycasts - 1)) * Mathf.PI;
            Vector3 direction = new Vector3(Mathf.Cos(angle_i), 0, Mathf.Sin(angle_i));
            Vector3 endPoint = transform.position + rayDistance * direction;

            Gizmos.DrawLine(transform.position, endPoint);
        }
    }
}
