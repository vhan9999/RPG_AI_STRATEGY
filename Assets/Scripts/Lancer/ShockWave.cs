using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : MonoBehaviour
{
    private int frame = 0;
    int up = 0;
    int left = 0;

    private void OnEnable()
    {
        frame = 0;
    }

    public void SetInitState(Vector2 initState)
    {
        up = (int)initState.x;
        left = (int)initState.y;
    }

    public void Move()
    {
        frame++;
        transform.localPosition += new Vector3(up * 0.015f * frame, -0.003f * Mathf.Pow(frame, 2), left * 0.08f);
    }
}
