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

    public bool IsSlash
    {
        get => anim.GetBool("isSlash");
        set => anim.SetBool("isSlash", value);
    }

    public bool IsAttack
    {
        get; set;
    }

    public void Slash()
    {
        if (!IsSlash)
        {
            RewardEvent.Invoke(-0.1f);
            IsSlash = true;
        }
    }

    // sword hit (compare tag)
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out WarriorAgent agent) && IsAttack)
        {
            Debug.Log("great");
            RewardEvent.Invoke(2f);
        }
    }

    public void ResetSlash()
    {
        IsSlash = false;
    }

    public void SetAttackState(int attackState)
    {
        IsAttack = (attackState != 0);
    }
}
