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
            if (weapon.IsAttack)
            {
                agent.currentHealth = agent.currentHealth > agent.health ? agent.health : agent.currentHealth + weapon.attackPower / 2;
                if (GameArgs.IsDense)
                    agent.AddReward(0.01f);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(!isHit)
            WeaponTouch(other);
    }

    public void ResetPush()
    {
        if (GameArgs.IsDense && !isHit)
            agent.AddReward(-0.03f);
        IsPush = false;
        isHit = false;
    }
}
