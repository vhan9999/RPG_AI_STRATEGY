using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class Weapon : MonoBehaviour
{
    protected Animator anim;
    public ClassAgent agent;
    public int attackPower;
    public bool isHit = false;
    public bool IsAttack = false;
    //private int attackCount = 0;
    //public float rewardRatio = 0;
    //protected int damage = 0;

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
        //if (!GameArgs.IsDense && damage != 0)
        //{
        //    agent?.AddReward(Math.Max(rewardRatio * (damage / 100f) * GameArgs.attack, -0.5f));
        //    Debug.Log(rewardRatio * (damage / 100f) * GameArgs.attack);
        //    damage = 0;
        //}
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
                    if (GameArgs.IsDense) agent.AddReward(GameArgs.GetRewardRatio(agent.profession, RewardType.Attack) * GameArgs.attack * 0.1f);
                    else agent.damage += attackPower;
                    otherAgent.TakeDamage(attackPower);
                }
                else
                {
                    //Debug.Log("Dont'hurt, you are his frend");
                    if (GameArgs.IsDense) agent.AddReward(-(GameArgs.GetRewardRatio(agent.profession, RewardType.Attack) * GameArgs.attack * 0.03f));
                    else agent.damage -= attackPower / 3;
                }
            }
            else if(other.TryGetComponent(out Wall wall))
            {
                isHit = true;
            }
        }
    }

    public void SetAttackState(int state)
    {
        IsAttack = state == 1 ? true : false;
    }
}
