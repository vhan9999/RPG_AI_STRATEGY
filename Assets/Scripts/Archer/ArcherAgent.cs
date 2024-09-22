using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class ArcherAgent : ClassAgent
{
    [SerializeField]
    private Bow bow;


    protected override void Awake()
    {
        base.Awake();
        bow = GetComponentInChildren<Bow>();
        bow.Reload();
        forwardSpeedMult = 0.8f;
        backSpeedMult = 1f;
    }

    private void Update()
    {
        Debug.Log(bow.fire);
        if (bp.BehaviorType == BehaviorType.HeuristicOnly)
        {
            if (Input.GetMouseButtonDown(0) && !bow.IsReloading)
            {

                bow.SetDrawingAnimation(true); // start drawing animation
                bow.fire = true;
            }
            if (bow.fire && bow.firePower < bow.maxFirePower)
            {
                bow.firePower += Time.deltaTime * bow.firePowerSpeed;
            }
            if (bow.fire && Input.GetMouseButtonUp(0))
            {
                bow.SetDrawingAnimation(false); // Stop drawing animation
                bow.Fire(bow.firePower);
                bow.firePower = 10;
                bow.fire = false;
            }
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor);
        sensor.AddObservation(bow.cooldownTime);
        sensor.AddObservation(bow.fire);
        sensor.AddObservation(bow.firePower);
    }
    public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
    {
        actionMask.SetActionEnabled(2, 1, !bow.IsReloading);
    }

    protected override void SpeedAdjust()
    {
        speed = bow.fire ? speed * 0.6f : speed;
    }

    protected override void AttackAction(int attackAction)
    {
        if (attackAction == 1 && !bow.IsReloading)
        {
            bow.SetDrawingAnimation(true); // start drawing animation
            bow.fire = true;
        }   
        else if(bow.fire)
        {
            if(GameArgs.IsDense)AddReward(((bow.firePower-10)/240)*(GameArgs.attack-1.5f));
            bow.SetDrawingAnimation(false); // Stop drawing animation
            bow.Fire(bow.firePower);
            bow.firePower = 10;
            bow.fire = false;
        }
    }
}
