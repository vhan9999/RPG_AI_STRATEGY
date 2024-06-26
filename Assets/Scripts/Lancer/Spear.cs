using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class Spear : MonoBehaviour
{
    public Animator anim;
    public ClassAgent agent;
    private bool IsAttack = false;

    private void Awake()
    {
        agent = GetComponentInParent<ClassAgent>();
    }

    public bool IsStab
    {
        get => anim.GetBool("isStab");
        set => anim.SetBool("isStab", value);
    }

    public void Stab()
    {
        if (!IsStab)
        {
            agent.AddReward(-0.2f);
            IsStab = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsAttack)
        {
            if (other.TryGetComponent(out ClassAgent otherAgent))
            {
                if (agent.team != otherAgent.team)
                {
                    //Debug.Log("great");
                    agent.AddReward(1f);
                    otherAgent.TakeDamage(15);
                }
                //else
                //{
                //    //Debug.Log("Dont'hurt, you are his frend");
                //    agent.AddReward(-0.3f);
                //}
            }
            //else if (other.TryGetComponent(out Wall wall))
            //{
            //    agent.AddReward(-0.3f);
            //}
        }
    }

    public void ResetStab()
    {
        IsStab = false;
    }

    public void SetAttackState(int attackState)
    {
        IsAttack = (attackState != 0);
    }
}
