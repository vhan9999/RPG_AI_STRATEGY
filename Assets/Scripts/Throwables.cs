using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwables : Weapon
{

    protected virtual void Start()
    {
        ffPenalty = 0.1f;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        
    }
}
