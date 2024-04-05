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
    private float executeTime = 5f;

    public bool IsAccelerate
    {
        get => anim.GetBool("isAccelerate");
        set => anim.SetBool("isAccelerate", value);
    }


    public void Execute()
    {
        if (!IsAccelerate)
        {
            CancelInvoke("Stop");
            anim.SetBool("isAccelerate", true);
            Invoke("Stop", executeTime);
        }
    }

    private void Stop()
    {
        anim.SetBool("isAccelerate", false);
    }
}
