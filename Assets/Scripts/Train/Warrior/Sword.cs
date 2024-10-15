using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class Sword : Weapon
{
    public AnimationClip slashAni;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip slashSound;

    private void Start()
    {
        attackDuration = slashAni.length;
        
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
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
            IsSlash = true;
            // Play slash sound
            if (slashSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(slashSound);
            }
        }
    }

    public void ResetSlash()
    {
        resetAttack();
        IsSlash = false;
        attackTime = 0;
    }
}
