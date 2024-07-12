using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;

using UnityEngine;

public class VikingAgent : ClassAgent
{
    // weapon
    private TwoAxes twoAxes;

    // skill
    private Accelerate accelerate;

    protected override void Awake()
    {
        base.Awake();
        twoAxes = GetComponentInChildren<TwoAxes>();
    }

    //private void Update()
    //{
    //    if (bp.BehaviorType == BehaviorType.HeuristicOnly)
    //    {
    //        if (Input.GetMouseButtonDown(0))
    //        {
    //            sword.Slash();
    //        }
    //        else if (Input.GetKeyDown(KeyCode.Z))
    //        {
    //        }
    //    }
    //}

    //public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
    //{
    //    //Debug.Log($"{!sword.IsThrust} {accelerate.IsAllowed}");
    //    actionMask.SetActionEnabled(3, 1, !sword.IsSlash);
    //    actionMask.SetActionEnabled(4, 1, accelerate.IsAllowed);
    //}

    //protected override void SpeedAdjust()
    //{
    //    speed = sword.IsSlash ? speed * 0.6f : speed;
    //    speed = accelerate.Status ? speed * 1.5f : speed;
    //}

    //protected override void AttackAction(int attackAction)
    //{
    //    if (attackAction == 1) sword.Slash();
    //}

    //protected override void SkillAction(int skillAction)
    //{
    //    if (skillAction == 1) accelerate.Execute();
    //}
}
