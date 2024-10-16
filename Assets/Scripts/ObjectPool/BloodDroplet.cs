using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodDroplet : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float lifetime = 2f;
    [SerializeField] private float minForce = 0.5f;
    [SerializeField] private float maxForce = 0.7f;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        ApplyRandomForce();
        Invoke("ReturnToPool", lifetime);
    }

    private void ApplyRandomForce()
    {
        Vector3 randomDirection = Random.insideUnitSphere.normalized;
        randomDirection.y = Mathf.Abs(randomDirection.y); // Ensure upward direction
        float randomForce = Random.Range(minForce, maxForce);
        rb.AddForce(randomDirection * randomForce, ForceMode.Impulse);
    }

    private void ReturnToPool()
    {
        BloodDropletPoolManager.Instance.ReturnToPool(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            ReturnToPool();
        }
    }

}
