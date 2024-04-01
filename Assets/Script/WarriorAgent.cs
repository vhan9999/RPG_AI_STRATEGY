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
    private float lerpSpeed = 5f;
    [SerializeField]
    private float maxSpeed = 10f; 
    private float speed = 10f;
    [SerializeField]
    public float rotateSpeed = 150f;
    private int rotateDir = 0;
    public Sword sword;
    [SerializeField] private Transform target;


    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(-6f, 7f), 1f, Random.Range(-7f, -4f));
        target.localPosition = new Vector3(Random.Range(-6f, 7f), 0.5f, Random.Range(2f, 6f));
    }
    
    private void Update()
    {
    
        if (Input.GetMouseButtonDown(0))
        {
            sword.Slash();
        }
        
    }

    private void FixedUpdate()
    {
        speed = Vector3.Angle(nowDir, transform.forward) < 80 ? maxSpeed : maxSpeed * 0.5f;
        speed = sword.anim.GetBool("isAttack") ? speed * 0.75f : speed;
        nowDir = Vector3.Lerp(nowDir, ctrlDir, lerpSpeed * Time.deltaTime);
        transform.Translate(nowDir *  Time.deltaTime * speed, Space.World);
        transform.Rotate(0f, rotateSpeed * Time.deltaTime * rotateDir, 0f);
        
    }
    public override void Initialize()
    {
        
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ctrlDir = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            ctrlDir += transform.forward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            ctrlDir -= transform.right.normalized;
        }
        if (Input.GetKey(KeyCode.S))
        {
            ctrlDir -= transform.forward.normalized;
        }
        if (Input.GetKey(KeyCode.D))
        {
            ctrlDir += transform.right.normalized;
        }
        ctrlDir = ctrlDir.normalized;

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
        int moveAction = actions.DiscreteActions[0];
        int rotateAction = actions.DiscreteActions[1];
        //int attackAction = actions.DiscreteActions[2];

        // move forward and backward
        if (moveAction == 1) ctrlDir += transform.forward;
        else if (moveAction == 2) ctrlDir -= transform.forward;

        // rotate
        if (rotateAction == 1) rotateDir = -1;
        else if (rotateAction == 2) rotateDir = 1;


        // attack
        if (actions.DiscreteActions[3] == 1) sword.Slash();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Agent's position
        sensor.AddObservation(transform.localPosition);

        // Target's position
        sensor.AddObservation(target.localPosition);

        // Agent's velocity
        sensor.AddObservation(GetComponent<Rigidbody>().velocity);

        // Agent's forward direction (assuming a flat environment, using only x and z)
        sensor.AddObservation(transform.forward.x);
        sensor.AddObservation(transform.forward.z);
    }


    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.CompareTag("Opponent")) && (sword.anim.GetBool("isAttack"))) {
            AddReward(10f);
            EndEpisode();
        }
        else if (other.gameObject.CompareTag("Wall"))
        {
            AddReward(-2f);
            EndEpisode();
        }

    }
}
