using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float existTime;

    public Vector3 moveDir;
    private float timer;
    public ClassAgent agent;
    private bool isHit = false;

    private void Start()
    {
       
    }

    public void Reset()
    {
        timer = 0;
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;
        timer += deltaTime;
        if (timer > existTime) { 
            ObjectPool<Arrow>.Instance.Recycle(this);
        }

        transform.Translate(moveDir * Time.deltaTime * speed, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ClassAgent otherAgent)) {
            if (agent.team != otherAgent.team)
            {
                //Debug.Log("great");
                agent.AddReward(1f);
                otherAgent.TakeDamage(10);
                ObjectPool<Arrow>.Instance.Recycle(this);
            }
            else
            {
                //Debug.Log("Dont'hurt, you are his frend");
                agent.AddReward(-0.3f);
            }
        }
        else if (other.TryGetComponent(out Wall wall))
        {
            ObjectPool<Arrow>.Instance.Recycle(this);
        }
    }
}
