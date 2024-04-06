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
    [SerializeField]private float lerpSpeed = 5f;
    [SerializeField]private float maxSpeed = 10f; 
    private float speed = 10f;
    [SerializeField]public float rotateSpeed = 150f;
    private int rotateDir = 0;
    
    // weapon
    public Sword sword;
    
    // skill
    [SerializeField]private Accelerate accelerate;
    [SerializeField]private WarCry warCry;
    public int HitCount = 0;
    [SerializeField] private Transform opponent_transform;
    public const int MaxHealth = 100;
    public int currentHealth = MaxHealth;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            currentHealth -= 10;
        }
        // skill 
        if (Input.GetMouseButtonDown(0))
        {
            sword.Slash();
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            accelerate.Execute();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            warCry.Execute();
        }
    }

    private void FixedUpdate()
    {
        if (Vector3.Angle(nowDir, transform.forward) > 120)
            speed = maxSpeed * 0.5f;
        else if (Vector3.Angle(nowDir, transform.forward) > 80)
            speed = maxSpeed * 0.75f;
        else
            speed = maxSpeed;
        speed = sword.anim.GetBool("isAttack") ? speed * 0.75f : speed;
        nowDir = Vector3.Lerp(nowDir, ctrlDir, lerpSpeed * Time.deltaTime);
        transform.Translate(nowDir *  Time.deltaTime * speed, Space.World);
        transform.Rotate(0f, rotateSpeed * Time.deltaTime * rotateDir, 0f);

    }

    public override void OnEpisodeBegin()
    {
        //transform.localPosition = new Vector3(Random.Range(-6f, 6f), 1.5f, Random.Range(-8f, -5f));
        //opponent_transform.localPosition = new Vector3(Random.Range(-6f, 6f), 1.5f, Random.Range(2f, 6f));
        //Debug.Log("Agent Spawn");
    }
    


    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // move
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

        // rotate
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
        int moveFrontBack = actions.DiscreteActions[0];
        int moveLeftRight = actions.DiscreteActions[1];
        int rotateAction = actions.DiscreteActions[2];
        int attackAction = actions.DiscreteActions[3];

        ctrlDir = Vector3.zero;
        // move forward and backward
        if (moveFrontBack == 1) ctrlDir += transform.forward;
        else if (moveFrontBack == 2) ctrlDir -= transform.forward;
        if (moveLeftRight == 1) ctrlDir += transform.right;
        else if (moveLeftRight == 2) ctrlDir -= transform.right;
        ctrlDir = ctrlDir.normalized;

        // rotate
        if (rotateAction == 1) rotateDir = -1;
        else if (rotateAction == 2) rotateDir = 1;


        // attack
        if (actions.DiscreteActions[3] == 1) sword.Slash();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        //// Agent's position
        //sensor.AddObservation(transform.localPosition);

        //// Target's position
        //sensor.AddObservation(opponent_transform.localPosition);

        //// Agent's velocity
        //sensor.AddObservation(GetComponent<Rigidbody>().velocity);

        //// Agent's forward direction (assuming a flat environment, using only x and z)
        //sensor.AddObservation(transform.forward.x);
        //sensor.AddObservation(transform.forward.z);
    }


    private void OnTriggerEnter(Collider other)
    {
        if ((other.transform == opponent_transform) && (sword.anim.GetBool("isAttack"))) {
            AddReward(10f);
            HitCount += 1;
            Debug.Log("Player Hit Target : "+ HitCount);
            EndEpisode();
        }
        else if (other.gameObject.CompareTag("Wall"))
        {
            AddReward(-2f);
            //EndEpisode();
        }

    }
}
