using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class MagicMissile : Weapon
{
    [SerializeField] private float speed;
    [SerializeField] private float existTime;
    public Vector3 moveDir;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        giveHurt = true;
    }

    public void Reset()
    {
        timer = 0;
    }
    private void OnEnable()
    {
        isHitHuman = false;
        isHitWall = false;
    }
    // Update is called once per frame
    void Update()
    {
        float deltaTime = Time.deltaTime;
        timer += deltaTime;
        if (timer > existTime) {
            //Destroy(gameObject);
            ObjectPool<MagicMissile>.Instance.Recycle(this);
        }

        transform.Translate(moveDir * Time.deltaTime * speed, Space.World);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if(isHitHuman) 
        {
            float distanse = Vector3.Distance(agent.transform.position, other.transform.position);
            if (GameArgs.IsDense) agent.AddReward(GameArgs.GetRewardRatio(agent.profession, RewardType.Attack) * GameArgs.rewardRatio * 0.02f * Math.Min(distanse / 8, 1f));
            else agent.damage += (int)(Math.Min(distanse / 8, 1f) * attackPower * 0.5f);
        }
        if (isHitHuman || isHitWall) ObjectPool<MagicMissile>.Instance.Recycle(this);
    }
}
