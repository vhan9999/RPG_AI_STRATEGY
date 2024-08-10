using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    protected Animator anim;
    public ClassAgent agent;
    protected bool IsAttack = false;
    public int attackPower;

    // Start is called before the first frame update
    protected virtual void Awake()
    {
        agent = GetComponentInParent<ClassAgent>();
        anim = GetComponent<Animator>();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (IsAttack)
        {
            if (other.TryGetComponent(out ClassAgent otherAgent))
            {
                if (agent.team != otherAgent.team)
                {
                    //Debug.Log("great");
                    agent.AddReward(1 * 0.5f);
                    otherAgent.TakeDamage(attackPower);
                }
                else
                {
                    //Debug.Log("Dont'hurt, you are his frend");
                    agent.AddReward(-0.3f * 0.5f);
                }
            }
            //else if (other.TryGetComponent(out Wall wall))
            //{
            //    agent.AddReward(-0.3f);
            //}
        }
    }

    public void SetAttackState(int attackState)
    {
        IsAttack = (attackState != 0);
    }
}
