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
<<<<<<< Updated upstream
=======
    
    // skill
    [SerializeField]
    private Accelerate accelerate;
    [SerializeField]
    private WarCry warCry;
    public int HitCount = 0;
    public const int MaxHealth = 100;
    public int currentHealth = MaxHealth;

>>>>>>> Stashed changes
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

    public override void OnEpisodeBegin()
    {
<<<<<<< Updated upstream
        
=======
        //transform.localPosition = new Vector3(Random.Range(-6f, 6f), 1.5f, Random.Range(-8f, -5f));
        //opponent_transform.localPosition = new Vector3(Random.Range(-6f, 6f), 1.5f, Random.Range(2f, 6f));
        //Debug.Log("Agent Spawn");
>>>>>>> Stashed changes
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
        
    }

<<<<<<< Updated upstream
    public override void CollectObservations(VectorSensor sensor)
    {
        
    }
=======

>>>>>>> Stashed changes

    // compare tag, whether hit ot not (reward)
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Character");
    }

}
