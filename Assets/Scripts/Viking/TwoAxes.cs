using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class TwoAxes : Weapon
{
    protected override void Awake()
    {
        base.Awake();
        SmallAxe[] smallAxes = GetComponentsInChildren<SmallAxe>();
        Debug.Log(smallAxes.Length);
        smallAxes[0].collideEvent.AddListener(OnTriggerEnter);
        smallAxes[1].collideEvent.AddListener(OnTriggerEnter);
    }

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
        if(GameArgs.IsDense && !isHit) agent.AddReward(-0.03f);
        isHit = false;
        IsCleave = false;
    }
}
