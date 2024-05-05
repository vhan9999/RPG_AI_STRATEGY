using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class Accelerate : MonoBehaviour
{
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private float executeTime = 2f;
    private ClassAgent agent;

    public bool IsAllowed { get; private set; }

    public bool Status
    {
        get => anim.GetBool("isAccelerate");
        set => anim.SetBool("isAccelerate", value);
    }

    private void Start()
    {
        agent = GetComponentInParent<ClassAgent>();
    }

    private void OnEnable()
    {
        IsAllowed = false;
        Status = false;
        CancelInvoke("EnableSkill");
        Invoke("EnableSkill", 5f);
    }

    private void EnableSkill()
    {
        IsAllowed = true;
    }

    public void Execute()
    {
        if (!Status)
        {
            IsAllowed = false;
            anim.SetBool("isAccelerate", true);
            Invoke("Stop", executeTime);
        }
    }

    private void Stop()
    {
        anim.SetBool("isAccelerate", false);
        Invoke("EnableSkill", 5f);
    }
}
