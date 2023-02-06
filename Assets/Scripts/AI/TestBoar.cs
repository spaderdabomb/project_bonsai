using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestBoar : MonoBehaviour
{
    public NavMeshAgent agent;

    private void Awake()
    {
        // agent.update
        agent.updateRotation = false;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(45f, 45f, 0);
    }
}
