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
    public Sword sword;

    // skill
    public Accelerate accelerate;

    protected override void Awake()
    {
        base.Awake();
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
            else if (Input.GetMouseButtonDown(1))
            {
                accelerate.Execute();
            }
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        base.OnActionReceived(actions);
        GameArgs.step++;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor);
        sensor.AddObservation(accelerate.cooldownTime);
        sensor.AddObservation(accelerate.Status);
        sensor.AddObservation(sword.IsSlash);
    }

    public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
    {
        //Debug.Log($"{!sword.IsThrust} {accelerate.IsAllowed}");
        actionMask.SetActionEnabled(2, 1, !sword.IsSlash);
        actionMask.SetActionEnabled(3, 1, accelerate.IsAllowed);
    }

    protected override void SpeedAdjust()
    {
        speed = sword.IsSlash ? speed * 0.6f : speed;
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
