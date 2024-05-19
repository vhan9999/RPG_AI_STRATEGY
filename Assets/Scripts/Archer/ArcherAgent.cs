using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class ArcherAgent : ClassAgent
{
    private Bow bow;

    protected override void Start()
    {
        base.Start();
        bow = GetComponentInChildren<Bow>();
        bow.Reload();
    }

    private void Update()
    {
        if (bp.BehaviorType == BehaviorType.HeuristicOnly)
        {
            if (Input.GetMouseButtonDown(0) && bow.IsReload == false)
            {
                bow.SetDrawingAnimation(true); // start drawing animation
                bow.IsAttack = true;
            }
            if (bow.IsAttack && bow.firePower < bow.maxFirePower)
            {
                bow.firePower += Time.deltaTime * bow.firePowerSpeed;
            }
            if (bow.IsAttack && Input.GetMouseButtonUp(0))
            {
                bow.SetDrawingAnimation(false); // Stop drawing animation
                bow.Fire(bow.firePower);
                bow.firePower = 0;
                bow.IsAttack = false;
            }
        }
    }

    public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
    {
        actionMask.SetActionEnabled(3, 1, !bow.IsAttack && bow.IsReload);
        actionMask.SetActionEnabled(4, 1, !bow.IsAttack && !bow.IsReload);
    }

    protected override void SpeedAdjust()
    {
        speed = bow.IsAttack ? speed * 0.6f : speed;
    }

    protected override void AttackAction(int attackAction)
    {
        if (attackAction == 1) bow.AttackShoot();
    }
}
