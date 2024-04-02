using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedAnim : MonoBehaviour
{
    [SerializeField]
    private Animator anim;

    public void Play()
    {
        anim.SetBool("IsPlay", true);
    }

    public void Stop()
    {
        anim.SetBool("IsPlay", false);
    }
}
