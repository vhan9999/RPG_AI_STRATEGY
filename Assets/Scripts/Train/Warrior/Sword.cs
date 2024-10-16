using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class Sword : Weapon
{
    // Audio components
    [SerializeField] private AudioSource audioSource;

    public AnimationClip slashAni;
    private void Start()
    {
        attackDuration = slashAni.length;
    }

    private void Update()
    {
        if (IsSlash)
        {
            if (attackTime < attackDuration)
                attackTime += Time.deltaTime;
            else
                attackTime = attackDuration;
        }
    }
    public bool IsSlash
    {
        get => anim.GetBool("isSlash");
        set => anim.SetBool("isSlash", value);
    }
    

    public void Slash()
    {
        if (!IsSlash)
        {
            audioSource?.Play();
            IsSlash = true;
        }
    }

    public void ResetSlash()
    {
        resetAttack();
        IsSlash = false;
        attackTime = 0;
    }
}
