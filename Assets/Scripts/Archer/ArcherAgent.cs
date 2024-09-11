using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
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
    }

    private void Update()
    {
        //Debug.Log(bow.IsReloading);
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

    public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
    {
        actionMask.SetActionEnabled(3, 1, !bow.fire && !bow.IsReloading);
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
            bow.SetDrawingAnimation(false); // Stop drawing animation
            bow.Fire(bow.firePower);
            bow.firePower = 0;
            bow.fire = false;
        }
    }
}
