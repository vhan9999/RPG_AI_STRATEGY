using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class Battleaxe : Weapons
{
    public bool IsAllowedWhirlwind = false;

    public bool IsCleave
    {
        get => anim.GetBool("isCleave");
        set => anim.SetBool("isCleave", value);
    }

    public bool IsWhirlwind
    {
        get => anim.GetBool("isWhirlwind");
        set => anim.SetBool("isWhirlwind", value);
    }

    private void OnEnable()
    {
        IsWhirlwind = false;
        IsAllowedWhirlwind = false;
        CancelInvoke("EnableWhirlwind");
        Invoke("EnableWhirlwind", 5f);
    }

    public void Cleave()
    {
        if (!IsCleave && !IsWhirlwind)
        {
            agent.AddReward(-0.2f);
            IsCleave = true;
        }
    }

    public void Whirlwind()
    {
        if (IsAllowedWhirlwind)
        {
            agent.AddReward(-0.3f);
            IsCleave = false;
            IsWhirlwind = true;
            IsAllowedWhirlwind = false;
            Invoke("StopWhirlwind", 3f);
        }
    }

    public void ResetCleave()
    {
        IsCleave = false;
    }

    public void StopWhirlwind()
    {
        IsWhirlwind = false;
        Invoke("EnableWhirlwind", 15f);
    }

    private void EnableWhirlwind()
    {
        IsAllowedWhirlwind = true;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (IsAttack || IsWhirlwind)
        {
            if (other.TryGetComponent(out ClassAgent otherAgent))
            {
                if (agent.team != otherAgent.team)
                {
                    //Debug.Log("great");
                    //agent.AddReward(IsCleave ? 1f : 0.3f);
                    otherAgent.TakeDamage(IsCleave ? attackPower : 8);
                }
                else
                {
                   //Debug.Log("Dont'hurt, you are his frend");
                   //agent.AddReward(IsCleave ? -0.3f : -0.1f);
                }
            }
            //else if (other.TryGetComponent(out Wall wall))
            //{
            //    agent.AddReward(IsCleave ? -0.3f : -0.1f);
            //}
        }
    }
}
