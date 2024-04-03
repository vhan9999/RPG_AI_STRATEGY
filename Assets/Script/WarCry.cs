using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarCry : MonoBehaviour
{
    [SerializeField]
    private Animator anim;

    public void Execute()
    {
        anim.SetTrigger("warCryTrigger");
        Debug.Log(3);
    }
}
