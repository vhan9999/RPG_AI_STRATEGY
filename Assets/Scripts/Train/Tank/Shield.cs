using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class Shield : Weapon
{
    public AnimationClip pushAni;
    private void Start()
    {
        attackDuration = pushAni.length;
    }
    private void Update()
    {
        if (IsPush)
        {
            if (attackTime < attackDuration)
                attackTime += Time.deltaTime;
            else
                attackTime = attackDuration;
        }
    }
    public bool IsPush
    {
        get => anim.GetBool("isPush");
        set => anim.SetBool("isPush", value);
    }

    public void Push()
    {
        
        if (!IsPush)
        {
            IsPush = true;
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Weapon weapon))
        {
            if (weapon.giveHurt)
            {
                agent.currentHealth = agent.currentHealth > agent.health ? agent.health : agent.currentHealth + weapon.attackPower / 2;
                if (GameArgs.IsDense)
                    agent.AddReward(0.01f);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isHitHuman && !isHitWall)
        {
            WeaponTouch(other);
            if (giveHurt && other.TryGetComponent(out ClassAgent otherAgent)) otherAgent.SlowDown();
        }
    }

    public void ResetPush()
    {
        resetAttack();
        IsPush = false;
        
        attackTime = 0;
    }
}
