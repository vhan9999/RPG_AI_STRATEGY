using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class Sword : MonoBehaviour
{
    [SerializeField]
    public Animator anim;
    private ClassAgent agent;
    private bool IsAttack = false;

    private void Start()
    {
        agent = GetComponentInParent<ClassAgent>();
    }

    public bool IsSlash
    {
        get => anim.GetBool("isSlash");
        set => anim.SetBool("isSlash", value);
    }

    public void Slash()
    {
        if (!IsSlash)
        {
            agent.AddReward(-0.03f);
            IsSlash = true;
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

    public void ResetSlash()
    {
        IsSlash = false;
    }

    public void SetAttackState(int attackState)
    {
        IsAttack = (attackState != 0);
    }
}
