using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using UnityEngine.SocialPlatforms.Impl;
using UnityEditor.Timeline.Actions;

public class TankAgent : ClassAgent
{
    // weapon
    public Shield shield;
    public WarCry warcry;

    protected override void Awake()
    {
        base.Awake();
        shield = GetComponentInChildren<Shield>();
        warcry = GetComponentInChildren<WarCry>();
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
            if (Input.GetMouseButtonDown(1))
            {
                Debug.Log("skill");
                warcry.Execute();
            }
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor);
        //sensor.AddObservation(warcry.cooldownTime);
        //sensor.AddObservation(shield.IsPush);
    }
    public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
    {
        //actionMask.SetActionEnabled(3, 1, !shield.IsPush);
    }
    protected override void SpeedAdjust()
    {
        speed = shield.IsPush ? speed * 0.5f : speed;
    }

    protected override void AttackAction(int attackAction)
    {
        if (attackAction == 1) shield.Push();
    }
    protected override void SkillAction(int skillAction)
    {
        if (skillAction == 1) warcry.Execute();
    }
}
