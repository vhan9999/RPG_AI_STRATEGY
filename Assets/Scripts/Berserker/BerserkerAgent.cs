using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using OpenCover.Framework.Model;

public class BerserkerAgent : ClassAgent
{
    // weapon
    private Battleaxe battleaxe;

    protected override void Start()
    {
        base.Start();
        battleaxe = GetComponentInChildren<Battleaxe>();
    }

    private void Update()
    {
        if (bp.BehaviorType == BehaviorType.HeuristicOnly)
        {
            if (Input.GetMouseButtonDown(0))
            {
                battleaxe.Cleave();
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                battleaxe.Whirlwind();
            }
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (battleaxe.IsWhirlwind)
        {
            transform.Rotate(0f, rotateSpeed * Time.deltaTime * 10, 0f);
        }
    }
    
    public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
    {
        actionMask.SetActionEnabled(3, 1, !battleaxe.IsCleave || !battleaxe.IsWhirlwind);
        actionMask.SetActionEnabled(2, 1, !battleaxe.IsWhirlwind);
        actionMask.SetActionEnabled(2, 2, !battleaxe.IsWhirlwind);
    }

    protected override void SpeedAdjust()
    {
        //speed = sword.IsSlash ? speed * 0.4f : speed;
    }

    protected override void AttackAction(int attackAction)
    {
        if (attackAction == 1) 
        { 
            battleaxe.Cleave(); 
        }
    }

    protected override void SkillAction(int skillAction)
    {
        if (skillAction == 1) 
        {
            battleaxe.Whirlwind();
            CancelInvoke("ResetWhirlwind");
            Invoke("ResetWhirlwind", 3f);
        }
    }
}
