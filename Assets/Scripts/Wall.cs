using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out ClassAgent agent))
        {
            //agent.TakeDamage(200);
            //agent.AddReward(-0.2f);
        }
        else if (other.TryGetComponent(out Weapon weapon))
        {
            //weapon.agent.AddReward(-1f);
        }
    }
}
