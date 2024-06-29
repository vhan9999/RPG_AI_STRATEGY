using System.Collections;
using Unity.MLAgents;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    [SerializeField]
    private Rigidbody rigid;

    public ClassAgent agent;

    private int damage;

    private void OnEnable()
    {
        ResetArrow();
    }

    private void ResetArrow()
    {
        rigid.isKinematic = false;
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }

    //public void SetEnemyTag(string enemyTag)
    //{
    //    this.enemyTag = enemyTag;
    //}

    public void Fly(Vector3 force)
    {
        rigid.isKinematic = false;
        rigid.AddForce(force, ForceMode.Impulse);
        damage = Mathf.Abs((int)force[2]);
        //Debug.Log(damage);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ClassAgent otherAgent)) {
            if (agent.team != otherAgent.team) {
                agent.AddReward(1f);
                otherAgent.GetComponent<ClassAgent>().TakeDamage(damage);
                //Debug.Log("Hit opponent");
                ObjectPool<Arrow>.Instance.Recycle(this);
            }
            else
            {
                //Debug.Log("Hit teammate");
                agent.AddReward(-0.3f);
                ObjectPool<Arrow>.Instance.Recycle(this);
            }
        }

        if (other.gameObject.CompareTag("Wall"))
        {
            agent.AddReward(-0.3f);
            ObjectPool<Arrow>.Instance.Recycle(this);
        }


        if (other.gameObject.CompareTag("RedArrow") || other.gameObject.CompareTag("BlueArrow"))
        {
            agent.AddReward(-0.3f);
            ObjectPool<Arrow>.Instance.Recycle(this);
        }

        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        rigid.isKinematic = true;
    }
}
