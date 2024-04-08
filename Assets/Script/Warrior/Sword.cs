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
            RewardEvent.Invoke(-0.1f);
            IsSlash = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out WarriorAgent agent) && IsAttack)
        {
            if (transform.parent.tag != agent.tag)
            {
                Debug.Log($"{transform.parent.tag} {agent.tag}");
                RewardEvent.Invoke(3f);
            }
            else
            {
                RewardEvent.Invoke(-1f);
            }
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
