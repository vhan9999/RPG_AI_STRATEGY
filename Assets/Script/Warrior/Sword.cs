using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class Sword : MonoBehaviour
{
    [SerializeField]
    public Animator anim;
    [HideInInspector]
    public UnityEvent<float> RewardEvent;
    [HideInInspector]
    public bool IsAttack = false;
    [HideInInspector]

    public bool IsSlash
    {
        get => anim.GetBool("isSlash");
        set => anim.SetBool("isSlash", value);
    }

    public void Slash()
    {
        if (!IsSlash)
        {
            RewardEvent.Invoke(-0.3f);
            IsSlash = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out WarriorAgent agent) && IsAttack)
        {
            Debug.Log("great");
            RewardEvent.Invoke(2f);
        }
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.TryGetComponent(out WarriorAgent agent) && IsAttack)
    //    {
    //        IsHurt = true;
    //    }
    //}

    public void ResetSlash()
    {
        IsSlash = false;
    }

    public void SetAttackState(int attackState)
    {
        IsAttack = (attackState != 0);
    }
}
