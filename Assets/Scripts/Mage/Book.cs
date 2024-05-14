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
    private FireBallCast fireBallCast;
    [SerializeField] private GameObject fireBall;
    [SerializeField] private GameObject magicMissile;

    private ObjectPool<MagicMissile> magicMissilePool;
    private ClassAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        fireBallCast = transform.GetChild(0).GetComponentInChildren<FireBallCast>();
        magicMissilePool = ObjectPool<MagicMissile>.Instance;
        magicMissilePool.InitPool(magicMissile, 30);
        agent = GetComponentInParent<ClassAgent>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnEnable()
    {
        IsCoolDown = false;
        IsSkill = false;
    }

    private void OnDisable()
    {
        fireBallCast.SetBack();
        CancelInvoke("AttackShoot");
        CancelInvoke("CoolDown");
    }
    public void AttackCast()
    {
        if (!IsAttack && !IsSkill)
        {
            IsAttack = true;
            Invoke("AttackShoot", 0.8f);
        }
    }

    private void AttackShoot()
    {
        
        MagicMissile m = magicMissilePool.Spawn(transform.position + transform.up, transform.rotation);
        m.tag = agent.team == Team.Blue ? "BlueMagicMissle" : "RedMagicMissle";
        m.moveDir = transform.forward;
        m.agent = agent;
        m.Reset();
        IsAttack = false;
    }

    public void FireBallCast()
    {
        if (!IsAttack && !IsCoolDown)
        {
            fireBallCast.CastStart();
            IsCoolDown = true;
            IsSkill = true;
            Invoke("CoolDown", 15f);
        }
    }

    private void CoolDown()
    {
        IsCoolDown = false;
    }

    public void FireBallShoot()
    {
        IsSkill = false;
        FireBall fireball = Instantiate(fireBall, transform.position + transform.up, transform.rotation).GetComponent<FireBall>();
        fireball.moveDir = transform.forward;
        fireball.agent = agent;
        fireBall.tag = agent.team == Team.Blue ? "BlueFireBall" : "RedFireBall";
    }
}
