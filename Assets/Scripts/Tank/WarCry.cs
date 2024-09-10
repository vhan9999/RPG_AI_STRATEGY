using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WarCry : MonoBehaviour
{
    [SerializeField]
    private Animator anim;
    private ClassAgent agent;
    private bool isAllowWarcry = true;

    private void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponentInParent<ClassAgent>();
    }

    public void Execute()
    {
        if (isAllowWarcry)
        {
            anim.SetTrigger("warCryTrigger");
            isAllowWarcry=false;
            Invoke("ResetWarcry", 1f);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ClassAgent otherAgent))
        {
            if (agent.team != otherAgent.team)
            {
                otherAgent.StartDizziness();
                if(GameArgs.IsDense)agent.AddReward(0.02f);
            }
        }
    }

    public void ResetWarcry()
    {
        isAllowWarcry = true;
    }
}
