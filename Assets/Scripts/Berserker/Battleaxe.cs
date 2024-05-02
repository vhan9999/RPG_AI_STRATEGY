using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class Battleaxe : MonoBehaviour
{
    [SerializeField]
    public Animator anim;
    private Agent agent;
    
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
        IsWhirlwind = true;
        Invoke("ResetWhiekwind", 3f);
    }

    public void ResetCleave()
    {
        IsCleave = false;
    }

    public void ResetWhiekwind()
    {
        IsWhirlwind = false;
    }
}
