using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class Spear : MonoBehaviour
{
    public Animator anim;
    public ClassAgent agent;
    private bool IsAttack = false;
    public bool IsAllowedSprint = true;

    [SerializeField]
    private GameObject shockWave;
    [SerializeField]
    private Transform SpawnPoint;

    private void Awake()
    {
        agent = GetComponentInParent<ClassAgent>();
        ObjectPool<ShockWave>.Instance.InitPool(shockWave, 20);
    }

    private void OnEnable()
    {
        IsSprint = false;
        IsAllowedSprint = false;
        CancelInvoke("EnableSprint");
        Invoke("EnableSprint", 5f);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ObjectPool<ShockWave>.Instance.Spawn(SpawnPoint.position, Quaternion.identity, transform);
            Debug.Log(3);
        }
    }

    public bool IsThrust
    {
        get => anim.GetBool("isThrust");
        set => anim.SetBool("isThrust", value);
    }

    public bool IsSprint
    {
        get => anim.GetBool("isSprint");
        set => anim.SetBool("isSprint", value);
    }

    public void Thrust()
    {
        if (!IsThrust)
        {
            agent.AddReward(-0.2f);
            IsThrust = true;
        }
    }

    public void Sprint()
    {
        if (IsAllowedSprint)
        {
            agent.AddReward(-0.3f);
            IsThrust = false;
            IsSprint = true;
            IsAllowedSprint = false;
            Invoke("StopSprint", 3f);
        }
    }

    private void StopSprint()
    {
        IsSprint = false;
        Invoke("EnableSprint", 15f);
    }


    private void EnableSprint()
    {
        IsAllowedSprint = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsAttack)
        {
            if (other.TryGetComponent(out ClassAgent otherAgent))
            {
                if (agent.team != otherAgent.team)
                {
                    //Debug.Log("great");
                    agent.AddReward(1f);
                    otherAgent.TakeDamage(15);
                }
                else
                {
                    //Debug.Log("Dont'hurt, you are his frend");
                    agent.AddReward(-0.3f);
                }
            }
            //else if (other.TryGetComponent(out Wall wall))
            //{
            //    agent.AddReward(-0.3f);
            //}
        }
    }

    public void ResetThrust()
    {
        IsThrust = false;
    }

    public void SetAttackState(int attackState)
    {
        IsAttack = (attackState != 0);
    }
}
