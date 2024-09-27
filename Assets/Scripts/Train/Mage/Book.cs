using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Book : MonoBehaviour
{
    [HideInInspector]
    public bool IsAttack = false;
    public bool IsSkill = false;
    public bool IsCoolDown = false;

    public float cooldown;
    public float cooldownTime = 0;
    public float skillDuration;
    public float skillTime = 0f;
    public float attackDuration;
    public float attackTime = 0f;

    private FireBallCast fireBallCast;
    [SerializeField] private GameObject fireBall;
    [SerializeField] private GameObject magicMissile;
    [SerializeField] private AnimationClip fireBallCastAni;

    private ObjectPool<MagicMissile> magicMissilePool;
    private ObjectPool<FireBall>fireBallPool;
    private ClassAgent agent;

    // Start is called before the first frame update
    void Awake()
    {
        fireBallCast = transform.GetChild(0).GetComponentInChildren<FireBallCast>();
        magicMissilePool = ObjectPool<MagicMissile>.Instance;
        magicMissilePool.InitPool(magicMissile, 8);
        fireBallPool = ObjectPool<FireBall>.Instance;
        fireBallPool.InitPool(fireBall, 1);
        agent = GetComponentInParent<ClassAgent>();

        attackDuration = 0.8f;
        cooldown = 6f;
        skillDuration = fireBallCastAni.length;
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldownTime > 0)
            cooldownTime -= Time.deltaTime;
        else
            cooldownTime = 0;
        if (skillTime < skillDuration)
            skillTime += Time.deltaTime;
        else
            skillTime = skillDuration;
        if (IsAttack)
        {
            if (attackTime < attackDuration)
                attackTime += Time.deltaTime;
            else
                attackTime = attackDuration;
        }
    }
    public void Revive()
    {
        IsAttack = false;
        IsSkill = false;
        IsCoolDown = false;
        CancelInvoke("AttackShoot");
        CancelInvoke("CoolDown");

    }

    public void AttackCast()
    {
        if (!IsAttack && !IsSkill)
        {
            IsAttack = true;
            Invoke("AttackShoot", attackDuration);
        }
    }

    private void AttackShoot()
    {
        MagicMissile m = magicMissilePool.Spawn(transform.position + transform.up, transform.rotation);
        m.tag = agent.team == Team.Blue ? "BlueMagicMissle" : "RedMagicMissle";
        //Debug.Log(m.tag);
        m.moveDir = transform.forward;
        m.agent = agent;
        m.Reset();
        IsAttack = false;
        attackTime = 0;
    }

    public void FireBallCast()
    {
        if (!IsAttack && !IsCoolDown)
        {
            //agent.AddReward(-0.03f);
            fireBallCast.CastStart();
            IsCoolDown = true;
            IsSkill = true;
            skillTime = 0f;
        }
    }

    private void CoolDown()
    {
        IsCoolDown = false;
    }

    public void FireBallShoot()
    {
        IsSkill = false;
        Invoke("CoolDown", cooldown);
        cooldownTime = cooldown;
        FireBall f = fireBallPool.Spawn(transform.position + transform.up, transform.rotation);
        f.tag = agent.team == Team.Blue ? "BlueMagicMissle" : "RedMagicMissle";
        f.moveDir = transform.forward;
        f.agent = agent;
        f.Reset();
    }
}
