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
using System.Reflection;
using UnityEngine.Rendering;
using System;

public class ClassAgent : Agent
{
    //move
    protected Vector3 nowDir = Vector3.zero;
    private Vector3 ctrlDir = Vector3.zero;
    [SerializeField]
    private float lerpSpeed = 5f;
    [SerializeField]
    private float maxSpeed = 10f;
    protected float speed = 10f;
    [SerializeField] 
    public float rotateSpeed = 150f;
    private int rotateDir = 0;
    private bool isDead = false;

    //private float randomX;
    //private float randomZ;

    private int hurtCount = 0;
    //public int count = 0;

    //health
    public int health;
    public int currentHealth;

    //team
    public Team team;
    public Profession profession; 
    protected BehaviorParameters bp;
    protected EnvController envController;
    protected Rigidbody rb;

    //init
    private Vector3 initPosition;
    private Quaternion initRotation;

    //state
    protected bool isDizzy = false;

    protected float sideSpeedMult = 0.75f;
    protected float forwardSpeedMult = 1f;
    protected float backSpeedMult = 0.5f;

    public float hpPct => (float)currentHealth / health * 100;

    protected virtual void Awake()
    {
        bp = GetComponent<BehaviorParameters>();
        
        team = (Team)bp.TeamId;
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        envController = GetComponentInParent<EnvController>();
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
        if (Vector3.Angle(nowDir, transform.forward) > 120)
            speed = maxSpeed * backSpeedMult;
        else if (Vector3.Angle(nowDir, transform.forward) > 80)
            speed = maxSpeed * sideSpeedMult;
        else
            speed = maxSpeed * forwardSpeedMult;
        speed = isDizzy ? speed * 0.3f : speed;
        SpeedAdjust();
        nowDir = Vector3.Lerp(nowDir, ctrlDir, lerpSpeed * Time.deltaTime);
        rb.AddForce(nowDir * Time.deltaTime * speed, ForceMode.VelocityChange);
        //rb.velocity = nowDir * Time.deltaTime * speed;
        transform.Rotate(0f, rotateSpeed * Time.deltaTime * rotateDir, 0f);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        hurtCount = 0;
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        AddReward(-0.08f * hurtCount * GameArgs.hurt);
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
        isDead = false;
        currentHealth = health;
        Debug.Log(team + " begin");
        float randomX = 0f;
        float randomZ = 0f;

        if (team == Team.Blue)
        {
            randomX = UnityEngine.Random.Range(-7f, 7f);
            randomZ = UnityEngine.Random.Range(-8f, -3f);
            Debug.Log("Blue position : " + randomX + "," + randomZ);
        }
        else if (team == Team.Red)
        {
            randomX = UnityEngine.Random.Range(-7f, 7f);
            randomZ = UnityEngine.Random.Range(3f, 9f);
            Debug.Log("Red position : " + randomX + "," + randomZ);
        }

        Vector3 randomLocalPosition = new Vector3(randomX, 1.8f, randomZ);
        transform.localPosition = randomLocalPosition;
        
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

    public override void OnActionReceived(ActionBuffers actions)
    {
        //if (!GameArgs.IsDense)
        //{
        //    if (++count >= 100)
        //    {
        //        count = 0;
        //    }
        //}
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
        //AddReward(-damage * (this is MageAgent ? 0.02f : 0.005f));
        //BloodDropletPoolManager.Instance.SpawnBloodDroplets(transform.position);
        currentHealth -= damage;
        HealthPenalty();
        if (currentHealth <= 0 && !isDead)
        {
            //Debug.Log(team+" Dead");
            isDead = true;
            gameObject.SetActive(false);
            envController?.DeadTouch(team);
        }
    }

    private void HealthPenalty()
    {
        if (GameArgs.IsDense)
        {
            AddReward(-1);
        }
        else
        {
            AddReward(-0.5f);
            //if (++hurtCount  % 2 == 0)
            //{

            //    AddReward(-0.8f);
            //}
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