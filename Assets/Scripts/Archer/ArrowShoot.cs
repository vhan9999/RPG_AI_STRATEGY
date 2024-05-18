using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShoot : MonoBehaviour
{

    private Bow bow;
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        bow = transform.GetComponent<Bow>();
    }

    public void CastStart()
    {
        anim.SetTrigger("fire");
    }

    public void CastDone()
    {
        bow.AttackShoot();
    }
}
