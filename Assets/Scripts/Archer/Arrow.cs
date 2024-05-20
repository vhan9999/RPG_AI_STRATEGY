using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField]
    private float damage;

    [SerializeField]
    private Rigidbody rigid;

    private string enemyTag;
    private bool didHit;

    private void OnEnable()
    {
        ResetArrow();
    }

    private void ResetArrow()
    {
        didHit = false;
        rigid.isKinematic = false;
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }

    public void SetEnemyTag(string enemyTag)
    {
        this.enemyTag = enemyTag;
    }

    public void Fly(Vector3 force)
    {
        rigid.isKinematic = false;
        rigid.AddForce(force, ForceMode.Impulse);
        transform.SetParent(null);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (didHit) return;
        didHit = true;

        if (other.gameObject.CompareTag("Wall"))
        {
            ObjectPool<Arrow>.Instance.Recycle(this);
        }
        else if (other.gameObject.CompareTag(enemyTag))
        {
            // Handle enemy hit logic
            ObjectPool<Arrow>.Instance.Recycle(this);
        }

        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        rigid.isKinematic = true;
        transform.SetParent(other.transform);
    }
}
