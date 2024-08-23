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

    private int attackCount = 0;

    protected virtual void Awake()
    {
        agent = GetComponentInParent<ClassAgent>();
        anim = GetComponent<Animator>();
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
                    DamageReward();
                    otherAgent.TakeDamage(attackPower);
                }
                else
                {
                    //Debug.Log("Dont'hurt, you are his frend");
                    if (GameArgs.IsDense) agent.AddReward(-ffPenalty);
                }
            }
        }
    }

    private void DamageReward()
    {
        if (GameArgs.IsDense)
        {
            agent.AddReward(1);
        }
        else
        {
            attackCount++;
            if (attackCount % 2 == 0)
            {
                Debug.Log(agent.team + " Hit Twice" + " Add "+ 0.5f * GameArgs.attack);
                agent.AddReward(0.5f * GameArgs.attack);
            }
        }
    } 

    public void SetAttackState(int state)
    {
        IsAttack = state == 1 ? true : false;
    }
}
