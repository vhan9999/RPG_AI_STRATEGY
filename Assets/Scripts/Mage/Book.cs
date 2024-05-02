using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour
{
    [HideInInspector]
    public bool IsAttack = false;
    [SerializeField] private GameObject magicMissile;

    private ObjectPool<MagicMissile> magicMissilePool;
    private ClassAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        magicMissilePool = ObjectPool<MagicMissile>.instance;
        magicMissilePool.InitPool(magicMissile, 30);
        agent = GetComponentInParent<ClassAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NormalAttack()
    {
        if (!IsAttack)
        {
            IsAttack = true;
            Invoke("ResetAttack", 0.5f);
        }
    }

    private void ResetAttack()
    {
        
        MagicMissile m = magicMissilePool.Spawn(transform.position + transform.up, transform.rotation);
        m.moveDir = transform.forward;
        m.agent = agent;
        m.Reset();
        IsAttack = false;
    }
}
