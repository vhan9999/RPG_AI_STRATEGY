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
        fireBallCast = transform.GetChild(0).GetChild(0).GetComponent<FireBallCast>();
        magicMissilePool = ObjectPool<MagicMissile>.instance;
        magicMissilePool.InitPool(magicMissile, 30);
        agent = GetComponentInParent<ClassAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AttactCast()
    {
        if (!IsAttack && !IsSkill)
        {
            IsAttack = true;
            Invoke("AttackShoot", 0.5f);
        }
    }

    private void AttackShoot()
    {
        
        MagicMissile m = magicMissilePool.Spawn(transform.position + transform.up, transform.rotation);
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
            Invoke("CoolDown", 10f);
        }
    }
    private void CoolDown()
    {
        IsCoolDown = false;
    }

    public void FireBallShoot()
    {
        IsSkill = false;
        GameObject f = Instantiate(fireBall, transform.position + transform.up, transform.rotation);
        f.GetComponent<FireBall>().moveDir = transform.forward;
        f.GetComponent<FireBall>().agent = agent;
    }
}
