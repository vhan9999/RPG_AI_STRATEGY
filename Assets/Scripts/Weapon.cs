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
    public bool isHitWall = false;
    public bool isHitHuman = false;
    public bool giveHurt = false;
    //private int attackCount = 0;
    //public float rewardRatio = 0;
    //protected int damage = 0;

    
    public float attackDuration;
    public float attackTime = 0f;
    protected virtual void Awake()
    {
        agent = GetComponentInParent<ClassAgent>();
        anim = GetComponent<Animator>();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        WeaponTouch(other);
    }

    public void WeaponTouch(Collider other)
    {
        if (giveHurt)
        {
            if (other.TryGetComponent(out ClassAgent otherAgent))
            {
                if (agent.team != otherAgent.team)
                {
                    //Debug.Log("great");
                    //agent.count = 0;
                    isHitHuman = true;
                    //
                    if (GameArgs.IsDense) agent.AddReward(GameArgs.GetRewardRatio(agent.profession, RewardType.Attack) * (2 - GameArgs.rewardRatio) * 0.1f * (attackPower/25f));
                    else agent.damage += attackPower;
                    otherAgent.TakeDamage(attackPower);
                }
                else
                {
                    //Debug.Log("Dont'hurt, you are his frend"); -1
                    //
                    if (GameArgs.IsDense) agent.AddReward(-(GameArgs.GetRewardRatio(agent.profession, RewardType.Attack) * (2 - GameArgs.rewardRatio) * 0.01f * (attackPower / 25f)));
                    else agent.damage -= attackPower / 5;
                }
            }
            else if (other.TryGetComponent(out Wall wall))
            {
                isHitWall = true;
                if (GameArgs.IsDense) agent.AddReward(-0.005f);
            }
        }
    }

    public void SetAttackState(int state)
    {
        giveHurt = state == 1 ? true : false;
    }

    //¶È­­¶i¾Ô
    public void resetAttack()
    {
        if (agent.envController is EnvControlleraaa)
        {
            ((EnvControlleraaa)(agent.envController)).DistanceReward(agent, -GameArgs.GetRewardRatio(agent.profession, RewardType.Attack) * 0.05f * (attackPower / 25f) * (GameArgs.rewardRatio / 2 + 0.5f));
        }
        isHitHuman = false;
        isHitWall = false;
    }
}
