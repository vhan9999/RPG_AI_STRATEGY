using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField]
    private float damage;

    [SerializeField]
    private Rigidbody rigid;
    
    private string eTag;
    private bool isHit;

    public ClassAgent agent;

    public void SetEnemyTag(string enemyTag) 
    {
        eTag = enemyTag;
    }

    public void Fly(Vector3 force) {
        rigid.isKinematic = false;
        rigid.AddForce(force, ForceMode.Impulse);
        transform.SetParent(null);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ClassAgent otherAgent))
        {
            if (agent.team != otherAgent.team) {
                isHit = true;
                agent.AddReward(1f);
                other.GetComponent<ClassAgent>().TakeDamage(20);
                ObjectPool<Arrow>.Instance.Recycle(this);
            }
            else
            {
                agent.AddReward(-0.3f);
            }
        }
        else if (other.gameObject.CompareTag("Wall"))
        {
            ObjectPool<Arrow>.Instance.Recycle(this);
        }
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        rigid.isKinematic=true;
        transform.SetParent(other.transform);
    }
}
