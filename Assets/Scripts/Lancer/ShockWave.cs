using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : MonoBehaviour
{
    private Vector3 speed;

    // Start is called before the first frame update
    void Start()
    {

    }


    private void OnEnable()
    {
        Invoke("Recycle", 1f);
        speed = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        speed += new Vector3(0.01f, 0.02f, 0.01f);
        Debug.Log(speed);
        transform.Translate(speed);
    }

    private void Recycle()
    {
        ObjectPool<ShockWave>.Instance.Recycle(this);
    }
}
