using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
            if (GameArgs.IsDense) agent.AddReward(-0.2f);
            IsPush = true;
        }
    }

    public void ResetPush()
    {
        Debug.Log("Reset Push");
        IsPush = false;
    }
}
