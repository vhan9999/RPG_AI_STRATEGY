using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    void Update()
    {
        //Debug.Log(Time.timeScale);
        // 如果按下空格鍵，則將時間流速設置為 0.5 倍
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("hi");
            Time.timeScale = 1f;
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            Time.timeScale = 40f;
        }
        // 如果按下 R 鍵，則恢復時間流速為正常速度
        if (Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 90f;
        }
    }
}