using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class Sword : Weapon
{
    public AnimationClip slashAni;
    private void Start()
    {
        attackDuration = slashAni.length;
    }

    private void Update()
    {
        if (IsSlash)
        {
            if (attackTime < attackDuration)
                attackTime += Time.deltaTime;
            else
                attackTime = attackDuration;
        }
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
            IsSlash = true;
        }
    }

    public void ResetSlash()
    {
        IsSlash = false;
        if (GameArgs.IsDense && !isHitHuman)
            agent.AddReward(-0.03f);
        isHitHuman = false;
        isHitWall = false;
        attackTime = 0;
    }
}
