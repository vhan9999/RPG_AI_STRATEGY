using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Sensors;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine;
using Unity.MLAgents;
using UnityEditor.Timeline.Actions;

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

    
    public int HitCount = 0;

    //health
    public int MaxHealth;
    public int currentHealth;

    //team
    protected BehaviorParameters bp;
    public Team team;
    public EnvController envController;

    //init
    private Vector3 initPosition;
    private Quaternion initRotation;

    

    public override void Initialize()
    {

        
        bp = GetComponent<BehaviorParameters>();
        team = (Team)bp.TeamId;
        initPosition = transform.localPosition;
        initRotation = transform.rotation;
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
        SpeedAdjust();
        nowDir = Vector3.Lerp(nowDir, ctrlDir, lerpSpeed * Time.deltaTime);
        transform.Translate(nowDir * Time.deltaTime * speed, Space.World);
        transform.Rotate(0f, rotateSpeed * Time.deltaTime * rotateDir, 0f);
    }
    protected virtual void SpeedAdjust()
    {

    }

    public override void OnEpisodeBegin()
    {
        currentHealth = MaxHealth;
        transform.localPosition = initPosition;
        transform.rotation = initRotation;
        //transform.localPosition = new Vector3(Random.Range(-6f, 6f), 1.5f, Random.Range(-8f, -5f));
        //opponent_transform.localPosition = new Vector3(Random.Range(-6f, 6f), 1.5f, Random.Range(2f, 6f));
        //Debug.Log("Agent Spawn");
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
        //int accelerateAction = actions.DiscreteActions[4];
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

        SkillAction(attackAction);

        //// skill
        //if (accelerateAction == 1) accelerate.Execute();
    }
    public virtual void SkillAction(int attackAction)
    {

    }



    private void OnTriggerEnter(Collider other)
    {
        if (currentHealth < 0)
            return;
        if (other.TryGetComponent(out Sword otherSword) && otherSword.IsAttack)
        {
            AddReward(-0.6f);
            currentHealth -= 30;
        }
        if(currentHealth < 0)
        {
            envController.DeadTouch(team);
        }
    }
}