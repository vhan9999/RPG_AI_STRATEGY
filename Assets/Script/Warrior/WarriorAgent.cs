using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;

public class WarriorAgent : ClassAgent
{
    // weapon
    private Sword sword;

    // skill
    private Accelerate accelerate;

    private int pridectAttackCount = 0;
    private int actualAttackCount = 0;

    protected override void Start()
    {
        base.Start();
        sword = GetComponentInChildren<Sword>();
        accelerate = GetComponentInChildren<Accelerate>();
    }

    private void Update()
    {
        if (bp.BehaviorType == BehaviorType.HeuristicOnly)
        {
            if (Input.GetMouseButtonDown(0))
            {
                sword.Slash();
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                accelerate.Execute();
            }
        }
    }

    protected override void SpeedAdjust()
    {
        base.SpeedAdjust();
        speed = sword.IsSlash ? speed * 0.4f : speed;
        speed = accelerate.IsAccelerate ? speed * 2 : speed;
    }

    public override void AttackAction(int attackAction)
    {
        if (!(sword.IsSlash && pridectAttackCount == 9))
        {
            pridectAttackCount++;
            actualAttackCount = attackAction == 1 ? actualAttackCount + pridectAttackCount : actualAttackCount - pridectAttackCount;
            if (pridectAttackCount == 10)
            {
                if (actualAttackCount > 0) sword.Slash();
                pridectAttackCount = 0;
                actualAttackCount = 0;
            }
        }
    }

    public override void SkillAction(int skillAction)
    {
        
    }
}
