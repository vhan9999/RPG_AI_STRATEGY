using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    void Update()
    {
        //Debug.Log(Time.timeScale);
        // �p�G���U�Ů���A�h�N�ɶ��y�t�]�m�� 0.5 ��
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("hi");
            Time.timeScale = 0.1f;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Time.timeScale = 40f;
        }
        // �p�G���U R ��A�h��_�ɶ��y�t�����`�t��
        if (Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 90f;
        }
    }
}