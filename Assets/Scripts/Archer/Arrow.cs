using System.Collections;
using Unity.MLAgents;
using UnityEngine;

public class Arrow : Weapon
{

    [SerializeField]
    private Rigidbody rigid;

    private int arrowDamage;

    private void Start()
    {
        IsAttack = true;
    }

    private void OnEnable()
    {
        ResetArrow();
    }

    private void ResetArrow()
    {
        rigid.isKinematic = false;
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }

    //public void SetEnemyTag(string enemyTag)
    //{
    //    this.enemyTag = enemyTag;
    //}

    public void Fly(Vector3 force)
    {
        rigid.isKinematic = false;
        rigid.AddForce(force, ForceMode.Impulse);
        arrowDamage = Mathf.Abs((int)force[2]);
        //Debug.Log(attackPower);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        attackPower = arrowDamage;

        base.OnTriggerEnter(other);
        if (isHit)
        {
            ObjectPool<Arrow>.Instance.Recycle(this);
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
            rigid.isKinematic = true;
        }
    }
}
