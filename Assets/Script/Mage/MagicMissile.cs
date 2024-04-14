using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMissile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float existTime;
    public Vector3 moveDir;
    private float timer;
    public Team team;
    // Start is called before the first frame update
    void Start()
    {
        
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
        if (timer > existTime) {
            //Destroy(gameObject);
            ObjectPool<MagicMissile>.instance.Recycle(this);
        }

        transform.Translate(moveDir * Time.deltaTime * speed, Space.World);
    }
}
