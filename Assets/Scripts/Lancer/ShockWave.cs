using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : MonoBehaviour
{
    public Vector3 Speed = Vector3.zero;
    public Vector3 Acceleration = Vector3.zero;

    private void OnEnable()
    {
        Speed = Vector3.zero;
        Acceleration = Vector3.zero;
    }

    public void Move()
    {
        Speed += Acceleration;
        transform.Translate(Speed);
    }
}
