using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    protected Animator anim;
    public ClassAgent agent;
    public int attackPower;
    public bool isHit = false;
    public bool IsAttack = false;
    public float ffPenalty = 0.3f;
    //private int attackCount = 0;
    public float rewardRatio = 0;
    private int damage = 0;

    protected virtual void Awake()
    {
        agent = GetComponentInParent<ClassAgent>();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        //attackCount = 0;
    }

    private void OnDisable()
    {
        if (!GameArgs.IsDense && damage != 0)
        {
            agent?.AddReward(rewardRatio * (damage / 100f) * GameArgs.attack);
            Debug.Log(rewardRatio * (damage / 100f) * GameArgs.attack);
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (IsAttack)
        {
            if (other.TryGetComponent(out ClassAgent otherAgent))
            {
                if (agent.team != otherAgent.team)
                {
                    //Debug.Log("great");
                    //agent.count = 0;
                    isHit = true;
                    if (GameArgs.IsDense) agent.AddReward(1);
                    damage += attackPower;
                    otherAgent.TakeDamage(attackPower);
                }
                else
                {
                    //Debug.Log("Dont'hurt, you are his frend");
                    damage -= attackPower;
                    if (GameArgs.IsDense) agent.AddReward(-ffPenalty);
                }
            }

        }
    }

    public void SetAttackState(int state)
    {
        IsAttack = state == 1 ? true : false;
    }
}
