using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using UnityEngine.SocialPlatforms.Impl;

public class TankAgent : ClassAgent
{
    // weapon
    private Shield shield;

    protected override void Awake()
    {
        base.Awake();
        shield = GetComponentInChildren<Shield>();
        penaltyRatio = 0.5f;
    }

    private void Update()
    {
        if (bp.BehaviorType == BehaviorType.HeuristicOnly)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("attack");
                shield.Push();
            }
        }
    }

    public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
    {
        actionMask.SetActionEnabled(3, 1, !shield.IsPush);
    }

    protected override void AttackAction(int attackAction)
    {
        if (attackAction == 1) shield.Push();
    }
}
