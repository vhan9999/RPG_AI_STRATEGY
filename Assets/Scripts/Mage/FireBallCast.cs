using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallCast : MonoBehaviour
{
    private Book book;
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        book = transform.GetComponentInParent<Book>();
    }

    public void CastStart()
    {
        anim.SetTrigger("fire");
    }
    public void CastDone()
    {
        book.Shoot();
    }
}
