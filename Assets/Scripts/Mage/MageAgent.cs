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
    private int pridectAttackCount = 0;
    private int actualAttackCount = 0;
    private Book book;

    protected override void Start()
    {
        base.Start();
        book = GetComponentInChildren<Book>();
    }

    private void Update()
    {
        if (bp.BehaviorType == BehaviorType.HeuristicOnly)
        {
            if (Input.GetMouseButtonDown(0))
            {
                book.AttackCast();
                Debug.Log("a");
            }
            else if (Input.GetMouseButtonDown(1))
            {
                book.FireBallCast();
            }
        }
    }

    public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
    {
        actionMask.SetActionEnabled(3, 1, !book.IsAttack && !book.IsCoolDown);
        actionMask.SetActionEnabled(4, 1, !book.IsAttack && !book.IsCoolDown);
    }

    protected override void SpeedAdjust()
    {
        speed = book.IsAttack ? speed * 0.6f : speed;
        speed = book.IsSkill ? speed * 0.3f : speed;
    }

    protected override void AttackAction(int attackAction)
    {
        if(attackAction == 1) book.FireBallShoot(); 
    }

    protected override void SkillAction(int skillAction)
    {
        if (skillAction == 1) book.FireBallCast();
    }
}
