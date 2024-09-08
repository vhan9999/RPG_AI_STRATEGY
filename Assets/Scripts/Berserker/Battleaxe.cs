using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class Battleaxe : Weapon
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
        IsAttack = false;
        IsWhirlwind = false;
        IsAllowedWhirlwind = false;
        CancelInvoke("EnableWhirlwind");
        Invoke("EnableWhirlwind", 5f);
    }

    public void Cleave()
    {
        if (!IsCleave && !IsWhirlwind)
        {
            if (GameArgs.IsDense) agent.AddReward(-0.2f);
            IsCleave = true;
        }
    }

    public void Whirlwind()
    {
        if (IsAllowedWhirlwind)
        {
            if (GameArgs.IsDense) agent.AddReward(-0.3f);
            IsCleave = false;
            IsAttack = true;
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
        IsAttack = false;
        IsWhirlwind = false;
        Invoke("EnableWhirlwind", 15f);
    }

    private void EnableWhirlwind()
    {
        IsAllowedWhirlwind = true;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (IsAttack)
        {
            if (other.TryGetComponent(out ClassAgent otherAgent))
            {
                if (agent.team != otherAgent.team)
                {
                    isHit = true;
                    if (GameArgs.IsDense) agent.AddReward(GameArgs.GetRewardRatio(agent.profession, RewardType.Attack) * GameArgs.attack * (IsCleave ? 0.1f : 0.05f));
                    else agent.damage += IsCleave ? attackPower : 8;
                    otherAgent.TakeDamage(IsCleave ? attackPower : 8);
                }
                else
                {

                    if (GameArgs.IsDense) agent.AddReward(-(GameArgs.GetRewardRatio(agent.profession, RewardType.Attack) * GameArgs.attack * 0.03f) * (IsCleave ? 1f : 0.2f));
                    else agent.damage -= (IsCleave ? attackPower : 8) / 3;
                }
            }
        }
    }
}
