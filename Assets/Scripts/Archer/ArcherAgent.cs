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
    [SerializeField]
    private Bow bow;


    protected override void Start()
    {
        base.Start();
        bow = GetComponentInChildren<Bow>();
        bow.Reload();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && bow.getIsReload==false)
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
            bow.firePower = 0;
            bow.fire = false; 
        }
    }

    public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
    {
        actionMask.SetActionEnabled(3, 1, !bow.fire && !bow.isReady);
        actionMask.SetActionEnabled(3, 1, !bow.fire && !bow.getIsReload);
    }

    protected override void SpeedAdjust()
    {
        speed = bow.fire ? speed * 0.6f : speed;
    }

    protected override void AttackAction(int attackAction)
    {
        if (attackAction == 1) bow.Fire(bow.firePower);
    }

}
