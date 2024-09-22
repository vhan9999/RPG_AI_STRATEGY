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
    private float inputSpeed = 0;
    [SerializeField] 
    public float rotateSpeed = 400f;
    private float rotateScale = 0;
    private bool isDead = false;

    //private int hurtCount = 0;

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
    [SerializeField] protected bool isDizzy = false;
    [SerializeField] protected bool isSlowDown = false;

    protected float sideSpeedMult = 0.75f;
    protected float forwardSpeedMult = 1f;
    protected float backSpeedMult = 0.5f;

    public float hpPct => (float)currentHealth / health * 100;

    [HideInInspector]
    public int damage = 0;
    public float rewardRatio;

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
        speed = isDizzy ? speed * 0.2f : speed;
        speed = isSlowDown ? speed * 0.75f : speed;
        SpeedAdjust();
        inputSpeed = speed;
        nowDir = Vector3.Lerp(nowDir, ctrlDir, lerpSpeed * Time.deltaTime);
        rb.AddForce(nowDir * Time.deltaTime * speed, ForceMode.VelocityChange);
        //rb.velocity = nowDir * Time.deltaTime * speed;
        transform.Rotate(0f, rotateSpeed * Time.deltaTime * rotateScale, 0f);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        //hurtCount = 0;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
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
        gameObject.GetComponent<CapsuleCollider>().enabled = true;
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<DecisionRequester>().enabled = true;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        if (bp.BehaviorType != BehaviorType.HeuristicOnly) return;
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        ActionSegment<int> dicreteActions = actionsOut.DiscreteActions;


        // move
        if (Input.GetKey(KeyCode.W))
        {
            dicreteActions[0] = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            dicreteActions[0] = 2;
        }
        if (Input.GetKey(KeyCode.D))
        {
            dicreteActions[1] = 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            dicreteActions[1] = 2;
        }

        // rotate
        if (Input.GetKey(KeyCode.Q))
        {
            continuousActions[0] = 0.4f;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            continuousActions[0] = -0.4f;
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        if(isDead) return;
        float rotateAction = actions.ContinuousActions[0];
        int moveFrontBack = actions.DiscreteActions[0];
        int moveLeftRight = actions.DiscreteActions[1];
        int attackAction = actions.DiscreteActions[2];
        int skillAction = actions.DiscreteActions[3];

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
        rotateScale = 0;
        rotateScale = rotateAction;

        if (isDizzy) return;
        if (GetComponent<BehaviorParameters>().BehaviorType == BehaviorType.HeuristicOnly) return;

        AttackAction(attackAction);

        SkillAction(skillAction);
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor);
        sensor.AddObservation(currentHealth);
        sensor.AddObservation(isDead);
        sensor.AddObservation(inputSpeed);
        sensor.AddObservation(isDizzy);
    }

    private void OnTriggerEnter(Collider other)
    {

    }

    public void GameOver()
    {
        isDead = true;
        if (!GameArgs.IsDense)
        {
            float reward = Math.Max(GameArgs.GetRewardRatio(profession, RewardType.Attack) * (damage / 100f) * GameArgs.attack, -0.5f)
            - (GameArgs.GetRewardRatio(profession, RewardType.Hurt) * (1f - (float)currentHealth / health) * GameArgs.hurt);
            AddReward(reward);
            Debug.Log(reward);
            damage = 0;
        }
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        transform.position =new Vector3(transform.position.x, -0.32f, transform.position.z);
        rotateScale = 0;
        ctrlDir = Vector3.zero;
        GetComponent<DecisionRequester>().enabled = false;
    }

    public void TakeDamage(int hurt)
    {
        //BloodDropletPoolManager.Instance.SpawnBloodDroplets(transform.position);
        currentHealth -= hurt;
        float dansePenalty = GameArgs.GetRewardRatio(profession, RewardType.Hurt) * GameArgs.hurt * 0.1f * (hurt / 25f);
        if (GameArgs.IsDense) AddReward(dansePenalty);
        if (currentHealth <= 0 && !isDead)
        {
            GameOver();
            envController?.DeadTouch(team);
        }

        if (profession != Profession.Tank)
            envController?.tankPenalty(team, dansePenalty);
    }

    public void StartDizziness()
    {
        isDizzy = true;
        CancelInvoke("Recover");
        Invoke("Recover", 1f);
    }

    public void Recover()
    {
        isDizzy = false;
    }

    public void SlowDown()
    {
        isSlowDown = true;
        CancelInvoke("ResetDown");
        Invoke("ResetSlowDown", 0.5f);
    }

    private void ResetSlowDown()
    {
        isSlowDown = false;
    }
}