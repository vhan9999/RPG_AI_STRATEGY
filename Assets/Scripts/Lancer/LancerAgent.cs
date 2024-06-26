using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using UnityEngine.SocialPlatforms.Impl;

public class LancerAgent : ClassAgent
{    
    // weapon
    private Spear spear;

    protected override void Awake()
    {
        base.Awake();
        spear = GetComponentInChildren<Spear>();
    }

    private void Update()
    {
        if (bp.BehaviorType == BehaviorType.HeuristicOnly)
        {
            if (Input.GetMouseButtonDown(0))
            {
                spear.Stab();
            }
        }
    }

    public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
    {
        actionMask.SetActionEnabled(3, 1, !spear.IsStab);
    }

    protected override void SpeedAdjust()
    {
        speed = spear.IsStab ? speed * 0.6f : speed;
    }

    protected override void AttackAction(int attackAction)
    {
        if (attackAction == 1) spear.Stab();
    }

    protected override void SkillAction(int skillAction)
    {

    }
}
