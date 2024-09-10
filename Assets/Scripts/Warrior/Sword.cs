using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class Sword : Weapon
{
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
        if (GameArgs.IsDense && !isHit)
            agent.AddReward(-0.03f);
        isHit = false;
    }
}
