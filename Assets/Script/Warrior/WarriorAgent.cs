using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;

public class WarriorAgent : ClassAgent
{
    // weapon
    public Sword sword;

    // skill
    [SerializeField]
    private Accelerate accelerate;
    [SerializeField]
    private WarCry warCry;

   
    private int pridectAttackCount = 0;
    private int actualAttackCount = 0;



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
            else if (Input.GetKeyDown(KeyCode.C))
            {
                warCry.Execute();
            }
        }
    }

    protected override void SpeedAdjust()
    {
        speed = sword.IsSlash ? speed * 0.75f : speed;
        speed = accelerate.IsAccelerate ? speed * 2 : speed;
    }

    public override void SkillAction(int attackAction)
    {
        if (!(sword.IsSlash && pridectAttackCount == 9))
        {
            pridectAttackCount++;
            actualAttackCount = attackAction == 1 ? actualAttackCount + pridectAttackCount : actualAttackCount - pridectAttackCount;
            if (pridectAttackCount == 10)
            {
                if (actualAttackCount > 0) sword.Slash();
                pridectAttackCount = 0;
                actualAttackCount = 0;
            }
        }
    }
}
