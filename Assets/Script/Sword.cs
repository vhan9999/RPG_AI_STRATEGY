using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField]
    public Animator anim;

    public void Slash()
    {
        anim.SetTrigger("attackTrigger");
    }

    // sword hit (compare tag)
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Sword");
    }
}
