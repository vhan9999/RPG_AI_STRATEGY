using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Sword : MonoBehaviour
{
    [SerializeField]
    public Animator anim;
    public bool isAttack = false;
    [HideInInspector]
    public UnityEvent<float> RewardEvent;

    public void Slash()
    {
        if (!isAttack)
        {
            anim.SetTrigger("attackTrigger");
            isAttack = true;
        }
    }

    // sword hit (compare tag)
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out WarriorAgent agent))
        {
            RewardEvent.Invoke(1f);
        }
    }

    public void Recovery()
    {
        isAttack = false;
    }
}
