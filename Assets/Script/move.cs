using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class move : Agent
{
    [SerializeField]
    private float speed = 1.0f;

    public override void Initialize()
    {
        
    }

    public override void OnEpisodeBegin()
    {
        
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(transform.forward * Time.deltaTime * speed );
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        
    }

}
