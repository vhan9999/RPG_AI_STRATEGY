using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Events;

public class SmallAxe : MonoBehaviour
{
    public UnityEvent<Collider> collideEvent;
    
    private void OnTriggerEnter(Collider other)
    {
        collideEvent.Invoke(other);
    }
}
