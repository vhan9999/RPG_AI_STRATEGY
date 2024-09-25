using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class Accelerate : MonoBehaviour
{
    [SerializeField]
    private Animator anim;

    public float cooldown;
    public float cooldownTime = 0f;
    public float skillDuration;
    public float skillTime = 0f;

    private ClassAgent agent;

    public bool IsAllowed { get; private set; }
    private void Start()
    {
        agent = GetComponentInParent<ClassAgent>();
        cooldown = 5f;
        skillDuration = 2f;
    }
    private void Update()
    {
        if (cooldownTime > 0)
            cooldownTime -= Time.deltaTime;
        else
            cooldownTime = 0;
        if (skillTime < skillDuration)
            skillTime += Time.deltaTime;
        else
            skillTime = skillDuration;
    }
    public bool Status
    {
        get => anim.GetBool("isAccelerate");
        set => anim.SetBool("isAccelerate", value);
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
        if (IsAllowed)
        {
            IsAllowed = false;
            anim.SetBool("isAccelerate", true);
            Invoke("Stop", skillDuration);
            skillTime = 0f;
        }
    }

    private void Stop()
    {
        anim.SetBool("isAccelerate", false);
        Invoke("EnableSkill", 5f);
        cooldownTime = 5f;
    }
}
