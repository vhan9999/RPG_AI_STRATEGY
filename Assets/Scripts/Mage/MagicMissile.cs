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
        IsAttack = true;
    }

    public void Reset()
    {
        timer = 0;
    }
    private void OnEnable()
    {
        isHit = false;
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
        if (isHit) ObjectPool<MagicMissile>.Instance.Recycle(this);
    }
}
