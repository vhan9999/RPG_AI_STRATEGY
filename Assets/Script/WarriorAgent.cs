using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class WarriorAgent : Agent
{
    private Vector3 nowDir = Vector3.zero;
    private Vector3 ctrlDir = Vector3.zero;
    [SerializeField]
    public float lerpSpeed = 5f;
    [SerializeField]
    public float speed = 5f;
    [SerializeField]
    public float rotateSpeed = 150f;
    private int rotateDir = 0;
    public Sword sword;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(3);
            sword.Slash();
        }
    }

    private void FixedUpdate()
    {
        nowDir = Vector3.Lerp(nowDir, ctrlDir, lerpSpeed * Time.deltaTime);
        transform.Translate(nowDir *  Time.deltaTime * speed, Space.World);
        transform.Rotate(0f, rotateSpeed * Time.deltaTime * rotateDir, 0f);
    }
    public override void Initialize()
    {
        
    }

    public override void OnEpisodeBegin()
    {
        
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        if (Input.GetKey(KeyCode.W) & Input.GetKey(KeyCode.D))
        {
            ctrlDir = (transform.forward + transform.right).normalized;
        }
        else if (Input.GetKey(KeyCode.D) & Input.GetKey(KeyCode.S))
        {
            ctrlDir = (transform.right - transform.forward).normalized;
        }
        else if (Input.GetKey(KeyCode.S) & Input.GetKey(KeyCode.A))
        {
            ctrlDir = (-transform.forward - transform.right).normalized;
        }
        else if (Input.GetKey(KeyCode.A) & Input.GetKey(KeyCode.W))
        {
            ctrlDir = (transform.forward - transform.right).normalized;
        }
        else if (Input.GetKey(KeyCode.W))
        {
            ctrlDir = transform.forward.normalized;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            ctrlDir = -transform.right.normalized;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            ctrlDir = -transform.forward.normalized;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            ctrlDir = transform.right.normalized;
        }
        else
        {
            ctrlDir = Vector3.zero;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            rotateDir = -1;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            rotateDir = 1;
        }
        else
        {
            rotateDir = 0;
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        
    }

}
