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
            if (GameArgs.IsDense) agent.AddReward(-0.2f);
            IsSlash = true;
        }
    }

    public void ResetSlash()
    {
        IsSlash = false;
    }
}
