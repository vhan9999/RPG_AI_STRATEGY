using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Sensors;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine;
using Unity.MLAgents;
using UnityEditor.Timeline.Actions;
using UnityEngine.Assertions;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using System.Collections.Generic;
using static UnityEngine.GraphicsBuffer;

public class ClassAgent : Agent
{
    //move
    private Vector3 nowDir = Vector3.zero;
    private Vector3 ctrlDir = Vector3.zero;
    [SerializeField]
    private float lerpSpeed = 5f;
    [SerializeField]
    private float maxSpeed = 10f;
    protected float speed = 10f;
    [SerializeField] 
    public float rotateSpeed = 150f;
    private int rotateDir = 0;

    //health
    public int health;
    public int currentHealth;

    //team
    public Team team;
    public Profession profession; 
    protected BehaviorParameters bp;
    protected EnvController envController;

    //init
    private Vector3 initPosition;
    private Quaternion initRotation;

    //state
    protected bool isDizzy = false;

    protected virtual void Start()
    {
        bp = GetComponent<BehaviorParameters>();
        envController = GetComponentInParent<EnvController>();
        team = (Team)bp.TeamId;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            currentHealth -= 10;
        }
    }

    protected virtual void FixedUpdate()
    {
        //if (this is BerserkerAgent)
        //{
        //    Debug.Log($"{speed} {maxSpeed} {lerpSpeed} {ctrlDir} {nowDir}");
        //}
        if (Vector3.Angle(nowDir, transform.forward) > 120)
            speed = maxSpeed * 0.5f;
        else if (Vector3.Angle(nowDir, transform.forward) > 80)
            speed = maxSpeed * 0.75f;
        else
            speed = maxSpeed;
        speed = isDizzy ? speed * 0.3f : speed;
        SpeedAdjust();
        nowDir = Vector3.Lerp(nowDir, ctrlDir, lerpSpeed * Time.deltaTime);
        transform.Translate(nowDir * Time.deltaTime * speed, Space.World);   
        transform.Rotate(0f, rotateSpeed * Time.deltaTime * rotateDir, 0f);
    }

    protected virtual void SpeedAdjust()
    {
        
    }

    protected virtual void AttackAction(int attackAction)
    {

    }

    protected virtual void SkillAction(int skillAction)
    {

    }

    public override void OnEpisodeBegin()
    {
        currentHealth = health;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        if (bp.BehaviorType != BehaviorType.HeuristicOnly) return;
        ActionSegment<int> actions = actionsOut.DiscreteActions;
        // move
        if (Input.GetKey(KeyCode.W))
        {
            actions[0] = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            actions[0] = 2;
        }
        if (Input.GetKey(KeyCode.D))
        {
            actions[1] = 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            actions[1] = 2;
        }

        // rotate
        if (Input.GetKey(KeyCode.Q))
        {
            actions[2] = 1;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            actions[2] = 2;
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(this is WarriorAgent ? 1 : -1);
        sensor.AddObservation(this is BerserkerAgent ? 1 : -1);
        sensor.AddObservation(this is MageAgent ? 1 : -1);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int moveFrontBack = actions.DiscreteActions[0];
        int moveLeftRight = actions.DiscreteActions[1];
        int rotateAction = actions.DiscreteActions[2];
        int attackAction = actions.DiscreteActions[3];
        int skillAction = actions.DiscreteActions[4];

        // move forward and backward
        ctrlDir = Vector3.zero;
        if (moveFrontBack == 1)
        {
            ctrlDir += transform.forward;
        }
        else if (moveFrontBack == 2)
        {
            ctrlDir -= transform.forward;
        }
        if (moveLeftRight == 1)
        {
            ctrlDir += transform.right;
        }
        else if (moveLeftRight == 2)
        {
            ctrlDir -= transform.right;
        }
        ctrlDir = ctrlDir.normalized;

        // rotate
        rotateDir = 0;
        if (rotateAction == 1)
        {
            rotateDir = -1;
        }
        else if (rotateAction == 2)
        {
            rotateDir = 1;
        }

        if (isDizzy) return;

        AttackAction(attackAction);

        SkillAction(skillAction);
    }

    private void OnTriggerEnter(Collider other)
    {

    }

    public void TakeDamage(int damage)
    {
        AddReward(-damage * 0.015f);
        //BloodDropletPoolManager.Instance.SpawnBloodDroplets(transform.position);
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);
            envController?.DeadTouch(team);
        }
    }

    public void StartDizziness()
    {
        isDizzy = true;
        Invoke("Recover", 3f);
    }

    public void Recover()
    {
        isDizzy = false;
    }
}