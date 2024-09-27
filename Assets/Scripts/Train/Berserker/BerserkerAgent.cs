using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using OpenCover.Framework.Model;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;

public class BerserkerAgent : ClassAgent
{
    // weapon
    public Battleaxe battleaxe;
    private Vector3 newDir = Vector3.zero;
    protected override void Awake()
    {
        base.Awake();
        battleaxe = GetComponentInChildren<Battleaxe>();
    }
    public override void OnEpisodeBegin()
    {
        base.OnEpisodeBegin();
        battleaxe.alive();
    }
    private void Update()
    {
        if (bp.BehaviorType == BehaviorType.HeuristicOnly)
        {
            if (Input.GetMouseButtonDown(0))
            {
                battleaxe.Cleave();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                newDir = nowDir;
                battleaxe.Whirlwind();
            }
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor);
        sensor.AddObservation(battleaxe.cooldownTime);
        sensor.AddObservation(battleaxe.IsCleave);
        sensor.AddObservation(battleaxe.IsWhirlwind);
    }
    public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
    {


        //actionMask.SetActionEnabled(2, 1, !battleaxe.IsWhirlwind);
        //actionMask.SetActionEnabled(2, 2, !battleaxe.IsWhirlwind);
        actionMask.SetActionEnabled(2, 1, !battleaxe.IsCleave && !battleaxe.IsWhirlwind);
        actionMask.SetActionEnabled(3, 1, battleaxe.IsAllowedWhirlwind);
    }

    protected override void SpeedAdjust()
    {
        speed = battleaxe.IsCleave ? speed * 0.6f : speed;
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
            if(battleaxe.IsAllowedWhirlwind)
                newDir = nowDir;
            battleaxe.Whirlwind();
        }
    }
}
