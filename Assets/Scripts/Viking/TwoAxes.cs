using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class TwoAxes : Weapon
{

    public bool IsCleave
    {
        get => anim.GetBool("isCleave");
        set => anim.SetBool("isCleave", value);
    }

    public void Cleave()
    {
        if (!IsCleave)
        {
            agent.AddReward(-0.2f);
            IsCleave = true;
        }
    }

    public void ResetCleave()
    {
        IsCleave = false;
    }
}
