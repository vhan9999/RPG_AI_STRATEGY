using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using UnityEngine;

public class RiderAgent : ClassAgent
{
    // weapon
    private TwoAxes twoAxes;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        if (bp.BehaviorType == BehaviorType.HeuristicOnly)
        {
            if (Input.GetMouseButtonDown(0))
            {

            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {

            }
        }
    }

    public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
    {
        //Debug.Log($"{!sword.IsThrust} {accelerate.IsAllowed}");
        //actionMask.SetActionEnabled(3, 1, !twoAxes.IsCleave);
    }

    protected override void SpeedAdjust()
    {

    }

    protected override void AttackAction(int attackAction)
    {

    }

    protected override void SkillAction(int skillAction)
    {

    }
}
