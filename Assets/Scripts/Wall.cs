using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.MLAgents;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //if (other.TryGetComponent(out ClassAgent agent))
        //{
        //    if (agent.profession == Profession.Warrior) {
        //        agent.TakeDamage(100);
        //    }
        //    if (agent.profession == Profession.Archer)
        //    {
        //        agent.TakeDamage(100);
        //    }
        //    if (agent.profession == Profession.Berserker)
        //    {
        //        agent.TakeDamage(120);
        //    }
        //    if (agent.profession == Profession.Tank)
        //    {
        //        agent.TakeDamage(100);
        //    }
        //    if (agent.profession == Profession.Mage)
        //    {
        //        agent.TakeDamage(80);
        //    }
        //}
    }

}
