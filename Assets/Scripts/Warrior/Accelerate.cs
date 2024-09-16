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
    public float cooldownTime = 0;
    private ClassAgent agent;

    public bool IsAllowed { get; private set; }

    void Update()
    {
        if (cooldownTime > 0)
            cooldownTime -= Time.deltaTime;
        else
            cooldownTime = 0;
    }
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
        IsAllowed = true;
        Status = false;
        CancelInvoke("EnableSkill");
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
        cooldownTime = 5f;
    }
}
