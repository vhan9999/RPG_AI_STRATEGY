using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    protected Animator anim;
    public ClassAgent agent;
    public int attackPower;
    public bool isHit = false;
    public bool IsAttack = false;
    public float ffPenalty = 0.3f;

    protected virtual void Awake()
    {
        agent = GetComponentInParent<ClassAgent>();
        anim = GetComponent<Animator>();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (IsAttack || this is Throwables)
        {
            if (other.TryGetComponent(out ClassAgent otherAgent))
            {
                if (agent.team != otherAgent.team)
                {
                    //Debug.Log("great");
                    isHit = true;
                    HealthReward();
                    otherAgent.TakeDamage(attackPower);
                }
                else
                {
                    //Debug.Log("Dont'hurt, you are his frend");
                    if (GameArgs.isDense) agent.AddReward(-ffPenalty);
                }
            }
        }
    }

    private void HealthReward()
    {
        if (GameArgs.isDense)
        {
            agent.AddReward(1);
        }
        else
        {
            if ((agent.hpPct > 75f && agent.hpPct - attackPower <= 75f) || (agent.hpPct > 50f && agent.hpPct - attackPower <= 50f))
            {
                agent.AddReward(0.5f);
            }
            else if (agent.hpPct > 25f && agent.hpPct - attackPower <= 25f)
            {
                agent.AddReward(0.8f);
            }
            else if (agent.hpPct > 0f && agent.hpPct - attackPower <= 0f)
            {
                agent.AddReward(1f);
            }
        }
    } 

    public void SetAttackState(int state)
    {
        IsAttack = state == 1 ? true : false; 
    }
}