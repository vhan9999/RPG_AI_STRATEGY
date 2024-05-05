using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class Battleaxe : MonoBehaviour
{
    [SerializeField]
    public Animator anim;
    private ClassAgent agent;
    private bool IsAttack = false;

    public bool IsAllowedWhirlwind = false;

    public bool IsCleave
    {
        get => anim.GetBool("isCleave");
        set => anim.SetBool("isCleave", value);
    }

    public bool IsWhirlwind
    {
        get => anim.GetBool("isWhirlwind");
        set => anim.SetBool("isWhirlwind", value);
    }

    private void Start()
    {
        agent = GetComponentInParent<ClassAgent>();
    }

    private void OnEnable()
    {
        IsWhirlwind = false;
        IsAllowedWhirlwind = false;
        CancelInvoke("EnableWhirlwind");
        Invoke("EnableWhirlwind", 5f);
    }

    public void Cleave()
    {
        if (!IsCleave && !IsWhirlwind)
        {
            agent.AddReward(-0.03f);
            IsCleave = true;
        }
    }

    public void Whirlwind()
    {
        if (IsAllowedWhirlwind)
        {
            IsCleave = false;
            IsWhirlwind = true;
            IsAllowedWhirlwind = false;
            Invoke("StopWhirlwind", 3f);
        }
    }

    public void ResetCleave()
    {
        IsCleave = false;
    }

    public void SetAttackState(int attackState)
    {
        IsAttack = (attackState != 0);
    }

    public void StopWhirlwind()
    {
        IsWhirlwind = false;
        Invoke("EnableWhirlwind", 20f);
    }

    private void EnableWhirlwind()
    {
        IsAllowedWhirlwind = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ClassAgent otherAgent) && (IsAttack || IsWhirlwind))
        {
            if (agent.team != otherAgent.team)
            {
                Debug.Log("great");
                agent.AddReward(1f);
                otherAgent.TakeDamage(IsCleave ? 20 : 8);
            }
            else
            {
                Debug.Log("Dont'hurt, you are his frend");
                agent.AddReward(IsCleave ? -0.3f : -0.1f);
            }
        }
    }
}
