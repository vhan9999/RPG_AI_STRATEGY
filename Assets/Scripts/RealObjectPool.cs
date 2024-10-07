using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealObjectPool : MonoBehaviour
{
    // Start is called before the first frame update
    public void RecycleAll()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.TryGetComponent(out Weapon weapon))
            {
                if (weapon is Arrow) ObjectPool<Arrow>.Instance.Recycle((Arrow)weapon);
                if (weapon is MagicMissile) ObjectPool<MagicMissile>.Instance.Recycle((MagicMissile)weapon);
                if (weapon is FireBall) ObjectPool<FireBall>.Instance.Recycle((FireBall)weapon);
            }
        }
    }
}
