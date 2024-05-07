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

    public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
    {
        //Debug.Log($"{!sword.IsSlash} {accelerate.IsAllowed}");
        actionMask.SetActionEnabled(3, 1, !sword.IsSlash);
        actionMask.SetActionEnabled(4, 1, accelerate.IsAllowed);
    }

    protected override void SpeedAdjust()
    {
        speed = sword.IsSlash ? speed * 0.4f : speed;
        speed = accelerate.Status ? speed * 1.5f : speed;
    }

    protected override void AttackAction(int attackAction)
    {
        if (attackAction == 1) sword.Slash();
    }

    protected override void SkillAction(int skillAction)
    {
        if (skillAction == 1) accelerate.Execute();
    }
}
