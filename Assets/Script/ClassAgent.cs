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

    private void FixedUpdate()
    {
        if (currentHealth < 0)
            return;
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
        if (currentHealth < 0)
            return;
        // move
        ctrlDir = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            ctrlDir += transform.forward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            ctrlDir -= transform.right;
        }
        if (Input.GetKey(KeyCode.S))
        {
            ctrlDir -= transform.forward;
        }
        if (Input.GetKey(KeyCode.D))
        {
            ctrlDir += transform.right;
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
        int skillAction = actions.DiscreteActions[4];
        if (currentHealth < 0)
            return;
        // move forward and backward
        if (GetComponent<BehaviorParameters>().BehaviorType != BehaviorType.HeuristicOnly) ctrlDir = Vector3.zero;
        if (moveFrontBack == 1) ctrlDir += transform.forward;
        else if (moveFrontBack == 2) ctrlDir -= transform.forward;
        if (moveLeftRight == 1) ctrlDir += transform.right;
        else if (moveLeftRight == 2) ctrlDir -= transform.right;
        ctrlDir = ctrlDir.normalized;

        // rotate
        if (rotateAction == 1) rotateDir = -1;
        else if (rotateAction == 2) rotateDir = 1;

        if (isDizzy) return;

        // attack
        AttackAction(attackAction);

        // skill
        SkillAction(skillAction);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Sword otherSword) && otherSword.IsAttack)
        {
            ClassAgent otherAgent = otherSword.GetComponentInParent<ClassAgent>();
            if (otherAgent != null && otherAgent.team != team)
            {
                AddReward(-0.2f);
                currentHealth -= 20;
            }
        }
        if(currentHealth <= 0)
        {
            gameObject.SetActive(false);
            envController.DeadTouch(team);
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