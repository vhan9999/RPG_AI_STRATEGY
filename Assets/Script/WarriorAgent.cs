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

    // weapon
    public Sword sword;
    
    // skill
    [SerializeField]
    private Accelerate accelerate;
    [SerializeField]
    private WarCry warCry;

    private void Start()
    {

    }

    private void Update()
    {
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
        speed = Vector3.Angle(nowDir, transform.forward) < 80 ? maxSpeed : maxSpeed * 0.5f;
        speed = sword.anim.GetBool("isAttack") ? speed * 0.75f : speed;
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
        
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        
    }

    // compare tag, whether hit ot not (reward)
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Character");
    }

}
