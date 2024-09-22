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

public class MageAgent : ClassAgent
{
    // skill
    public Book book;

    protected override void Awake()
    {
        base.Awake();
        book = GetComponentInChildren<Book>();
        forwardSpeedMult = 0.8f;
        backSpeedMult = 1f;
    }

    public override void OnEpisodeBegin()
    {
        base.OnEpisodeBegin();
        book.Revive();
    }
    private void Update()
    {
        if (bp.BehaviorType == BehaviorType.HeuristicOnly)
        {
            if (Input.GetMouseButtonDown(0))
            {
                book.AttackCast();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                book.FireBallCast();
            }
        }
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor);
        sensor.AddObservation(book.cooldownTime);
        sensor.AddObservation(book.IsSkill);
        sensor.AddObservation(book.IsAttack);
    }
    public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
    {
        actionMask.SetActionEnabled(2, 1, !book.IsAttack && !book.IsSkill);
        actionMask.SetActionEnabled(3, 1, !book.IsAttack && !book.IsCoolDown);
    }

    protected override void SpeedAdjust()
    {
        speed = book.IsAttack ? speed * 0.6f : speed;
        speed = book.IsSkill ? speed * 0.3f : speed;
    }

    protected override void AttackAction(int attackAction)
    {
        if(attackAction == 1) book.AttackCast(); 
    }

    protected override void SkillAction(int skillAction)
    {
        if (skillAction == 1) book.FireBallCast();
    }
}
