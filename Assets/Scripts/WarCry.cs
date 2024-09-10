using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WarCry : MonoBehaviour
{
    [SerializeField]
    private Animator anim;
    private ClassAgent agent;

    private void Start()
    {
        agent = GetComponentInParent<ClassAgent>();
    }

    public void Execute()
    {
        anim.SetTrigger("warCryTrigger");
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
}
