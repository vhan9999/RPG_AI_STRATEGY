using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;

public class FireBall : Weapon
{
    [SerializeField] private float speed;
    [SerializeField] private float existTime;
    public Vector3 moveDir;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        ffPenalty = 0.3f;
        IsAttack = true;
    }
    
    public void Reset()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float deltaTime = Time.deltaTime;
        timer += deltaTime;
        if (timer > existTime)
        {
            ObjectPool<FireBall>.Instance.Recycle(this);
        }
        if(!isHit)
            transform.Translate(moveDir * Time.deltaTime * speed, Space.World);
    }
    
    private void OnEnable()
    {
        isHit = false;
        anim.SetBool("touch", false);
        anim.SetTrigger("new");
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (isHit) anim.SetBool("touch", true);
    }

    public void ExplodeDone()
    {
        ObjectPool<FireBall>.Instance.Recycle(this);
    }
}

