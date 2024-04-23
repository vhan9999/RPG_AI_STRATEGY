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

    private bool isWhirlwind = false;

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
                
            }
        }
    }

    public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
    {
        //Debug.Log($"{!sword.IsSlash} {accelerate.IsAllowed}");
        //actionMask.SetActionEnabled(3, 1, !sword.IsSlash);
        //actionMask.SetActionEnabled(4, 1, accelerate.IsAllowed);
    }

    protected override void SpeedAdjust()
    {
        //speed = sword.IsSlash ? speed * 0.4f : speed;
    }

    protected override void AttackAction(int attackAction)
    {
        //if (!(sword.IsSlash && pridectAttackCount == 9))
        //{
        //    pridectAttackCount++;
        //    actualAttackCount = attackAction == 1 ? actualAttackCount + pridectAttackCount : actualAttackCount - pridectAttackCount;
        //    if (pridectAttackCount == 10)
        //    {
        //        if (actualAttackCount > 0) sword.Slash();
        //        pridectAttackCount = 0;
        //        actualAttackCount = 0;
        //    }
        //}
        if (attackAction == 1) 
        { 
            battleaxe.Cleave(); 
        }
    }

    protected override void SkillAction(int skillAction)
    {
        if (skillAction == 1) 
        { 
            isWhirlwind = true;
            CancelInvoke("ResetWhirlwind");
            Invoke("ResetWhirlwind", 3f);
        }
    }

    private void ResetWhirlwind()
    {
        isWhirlwind = false;
    }
}
