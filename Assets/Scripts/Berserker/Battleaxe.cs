using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class Battleaxe : Weapon
{
    public bool IsAllowedWhirlwind = false;
    public float cooldownTime = 0;

    void Update()
    {
        if (cooldownTime > 0)
            cooldownTime -= Time.deltaTime;
        else
            cooldownTime = 0;
    }

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

    public void alive()
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
            IsCleave = true;
        }
    }

    public void Whirlwind()
    {
        if (IsAllowedWhirlwind)
        {
            IsCleave = false;
            IsAttack = true;
            IsWhirlwind = true;
            IsAllowedWhirlwind = false;
            Invoke("StopWhirlwind", 3f);
        }
    }

    public void ResetCleave()
    {
        if (GameArgs.IsDense && !isHitHuman) agent.AddReward(-0.03f);
        isHitHuman = false;
        isHitWall = false;
        IsCleave = false;
    }

    public void StopWhirlwind()
    {
        IsAttack = false;
        IsWhirlwind = false;
        Invoke("EnableWhirlwind", 15f);
        cooldownTime = 15f;
    }

    private void EnableWhirlwind()
    {
        IsAllowedWhirlwind = true;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        attackPower = IsCleave ? 30 : 8;
        base.OnTriggerEnter(other);
    }
}
