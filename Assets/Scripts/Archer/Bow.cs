using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bow : MonoBehaviour
{
    [HideInInspector]
    public bool IsAttack = false;
    public bool IsReload = false;

    [SerializeField] private GameObject arrow;

    private ObjectPool<Arrow> arrowPool;
    private ClassAgent agent;

    private void Start()
    {
        arrowPool = ObjectPool<Arrow>.Instance;
        arrowPool.InitPool(arrow, 30);
        agent = GetComponentInParent<ClassAgent>();
    }

    private void Update()
    {
        
    }

    public void AttackCast() {
        if (!IsAttack) {
            IsAttack = true;
            Invoke("AttackShoot", 0.8f);
        }
    }

    public void AttackShoot() {
        Arrow a = arrowPool.Spawn(transform.position + transform.up, transform.rotation);
        a.tag = agent.team == Team.Blue ? "BlueArrow" : "RedArrow";
        a.moveDir = transform.forward;
        a.agent = agent;
        a.Reset();
        IsAttack = false;
    }

    public void Reload() { 
        IsReload = false;
    }
}
