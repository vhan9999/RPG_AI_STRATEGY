using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object colliding with the ground is a blood droplet
        if (other.gameObject.CompareTag("Blood"))
        {
            Debug.Log(3);
            //BloodDropletPoolManager.Instance.ReturnToPool(other.gameObject);
        }
    }
}