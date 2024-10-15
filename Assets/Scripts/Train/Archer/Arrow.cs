using System;
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
        giveHurt = true;
    }

    private void OnEnable()
    {
        ResetArrow();
        isHitHuman = false;
        isHitWall = false;
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

        if (isHitHuman)
        {
            float distanse = Vector3.Distance(agent.transform.position, other.transform.position);
            
            if (GameArgs.IsDense) agent.AddReward(GameArgs.GetRewardRatio(agent.profession, RewardType.Attack) * GameArgs.rewardRatio * 0.1f * Math.Min(distanse / 12, 1f));
            else agent.damage += (int)(Math.Min(distanse / 12, 1f) * attackPower * 0.5f);

        }
        base.OnTriggerEnter(other);
        if (isHitWall || isHitHuman)
        {
            ObjectPool<Arrow>.Instance.Recycle(this);
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
            rigid.isKinematic = true;
        }
    }
}
