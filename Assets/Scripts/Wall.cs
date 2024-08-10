using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        //if (other.TryGetComponent(out FireBall fireball))
        //{
        //    ObjectPool<FireBall>.Instance.Recycle(fireball);
        //}
        //else if (other.TryGetComponent(out MagicMissile magicMissile))
        //{
        //    ObjectPool<MagicMissile>.Instance.Recycle(magicMissile);
        //}
        //else if (other.TryGetComponent(out Battleaxe battleAxe))
        //{
        //    battleAxe.agent.AddReward(-1f);
        //}
        //else if (other.TryGetComponent(out Sword sword))
        //{
        //    sword.agent.AddReward(-1f);
        //}
    }
}
