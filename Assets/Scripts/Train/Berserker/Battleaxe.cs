using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class Battleaxe : Weapon
{
    public bool IsAllowedWhirlwind = false;
    public AnimationClip cleaveAni;
    public float cooldown;
    public float cooldownTime = 0f;
    public float skillDuration;
    public float skillTime = 0f;

    private void Start()
    {
        attackDuration = cleaveAni.length;
        cooldown = 15f;
        skillDuration = 3f;
    }

    void Update()
    {
        if (cooldownTime > 0)
            cooldownTime -= Time.deltaTime;
        else
            cooldownTime = 0;
        if (skillTime < skillDuration)
            skillTime += Time.deltaTime;
        else
            skillTime = skillDuration;
        if (IsCleave) {
            if (attackTime < attackDuration)
                attackTime += Time.deltaTime;
            else
                attackTime = attackDuration;
        }
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
        giveHurt = false;
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
            giveHurt = true;
            IsWhirlwind = true;
            IsAllowedWhirlwind = false;
            Invoke("StopWhirlwind", skillDuration);
            skillTime = 0f;
        }
    }

    public void ResetCleave()
    {
        resetAttack();
        IsCleave = false;
        attackTime = 0;
    }

    public void StopWhirlwind()
    {
        giveHurt = false;
        IsWhirlwind = false;
        Invoke("EnableWhirlwind", cooldown);
        cooldownTime = cooldown;
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
