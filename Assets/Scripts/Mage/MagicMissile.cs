using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class MagicMissile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float existTime;
    public Vector3 moveDir;
    private float timer;
    [HideInInspector]
    public ClassAgent agent;

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
            ObjectPool<MagicMissile>.Instance.Recycle(this);
        }

        transform.Translate(moveDir * Time.deltaTime * speed, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ClassAgent otherAgent))
        {
            if (agent.team != otherAgent.team)
            {
                //Debug.Log("great");
                agent.AddReward(0.5f + Mathf.Min(timer / 2.5f, 0.5f));
                otherAgent.TakeDamage(10);
                ObjectPool<MagicMissile>.Instance.Recycle(this);
            }
            else
            {
                //Debug.Log("Dont'hurt, you are his frend");
                agent.AddReward(-0.3f);
            }
        }
    }
}
