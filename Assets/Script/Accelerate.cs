using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accelerate : MonoBehaviour
{
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private float executeTime = 5f;

    public void Execute()
    {
        CancelInvoke("Stop");
        anim.SetBool("isAccelerate", true);
        Invoke("Stop", executeTime);
    }

    private void Stop()
    {
        anim.SetBool("isAccelerate", false);
    }
}
