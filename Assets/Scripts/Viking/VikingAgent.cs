using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;

using UnityEngine;

public class VikingAgent : ClassAgent
{
    // weapon
    private TwoAxes twoAxes;

    protected override void Awake()
    {
        base.Awake();
        twoAxes = GetComponentInChildren<TwoAxes>();
    }

    private void Update()
    {
        if (bp.BehaviorType == BehaviorType.HeuristicOnly)
        {
            if (Input.GetMouseButtonDown(0))
            {
                twoAxes.Cleave();
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {

            }
        }
    }

    public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
    {
        //Debug.Log($"{!sword.IsThrust} {accelerate.IsAllowed}");
        actionMask.SetActionEnabled(3, 1, !twoAxes.IsCleave);
    }

    protected override void SpeedAdjust()
    {
        speed = twoAxes.IsCleave ? speed * 0.8f : speed;
    }

    protected override void AttackAction(int attackAction)
    {
        if (attackAction == 1) twoAxes.Cleave();
    }

    protected override void SkillAction(int skillAction)
    {

    }
}
