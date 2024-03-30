using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField]
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && anim.GetBool("isAttack"))
        {
            anim.SetBool("isAttack", false);
        }
    }

    public void Slash()
    {
        anim.SetBool("isAttack", true);
    }
}
