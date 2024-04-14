using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;

public class MageAgent : ClassAgent
{

    // skill

    [SerializeField] private GameObject magicMissile;
    
    private ObjectPool<MagicMissile> magicMissilePool;
    private int pridectAttackCount = 0;
    private int actualAttackCount = 0;

    protected override void Start()
    {
        base.Start();
        magicMissilePool = ObjectPool<MagicMissile>.instance;
        magicMissilePool.InitPool(magicMissile, 30);
    }

    private void Update()
    {
        if (bp.BehaviorType == BehaviorType.HeuristicOnly)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //GameObject m = Instantiate(magicMissile, transform.position + transform.forward * 1, transform.rotation);
                MagicMissile m = magicMissilePool.Spawn(transform.position + transform.forward * 1, transform.rotation);
                m.moveDir = transform.forward;
                m.team = Team.Blue;
                m.Reset();
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                
            }
        }
    }

    public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
    {
        
    }

    protected override void SpeedAdjust()
    {
        //speed = sword.IsSlash ? speed * 0.4f : speed;
        //speed = accelerate.Status ? speed * 1.5f : speed;
    }

    protected override void AttackAction(int attackAction)
    {
        //if (!(sword.IsSlash && pridectAttackCount == 9))
        //{
        //    pridectAttackCount++;
        //    actualAttackCount = attackAction == 1 ? actualAttackCount + pridectAttackCount : actualAttackCount - pridectAttackCount;
        //    if (pridectAttackCount == 10)
        //    {
        //        if (actualAttackCount > 0) sword.Slash();
        //        pridectAttackCount = 0;
        //        actualAttackCount = 0;
        //    }
        //}
        //if (attackAction == 1) { sword.Slash(); }
    }

    protected override void SkillAction(int skillAction)
    {
        MagicMissile m = magicMissilePool.Spawn(transform.position + transform.forward * 1, transform.rotation);
        m.moveDir = transform.forward;
        m.Reset();
    }
}
