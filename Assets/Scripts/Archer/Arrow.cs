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
    private bool didHit;
    
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
        if (didHit) return;
        didHit = true;

        if (other.gameObject.CompareTag("Wall")) {
            Destroy(gameObject);
        }
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        rigid.isKinematic=true;
        transform.SetParent(other.transform);
    }
}
