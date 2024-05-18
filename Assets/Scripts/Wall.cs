using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out FireBall fireball))
        {
            Debug.Log(3);
            ObjectPool<FireBall>.Instance.Recycle(fireball);       
        }
        else if (other.TryGetComponent(out MagicMissile magicMissile))
        {
            ObjectPool<MagicMissile>.Instance.Recycle(magicMissile);
        }
    }
}
