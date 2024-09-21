using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthBar : MonoBehaviour
{

    public GameObject Player = null;


    public RectTransform Health, Hurt;

    void Update()
    {

        if (Player != null)
        {
            Health.sizeDelta = new Vector2((Player.GetComponent<ClassAgent>().currentHealth/ Player.GetComponent<ClassAgent>().health)*100, Health.sizeDelta.y);

            if (Hurt.sizeDelta.x > Health.sizeDelta.x)
            {
                Hurt.sizeDelta += new Vector2(-1, 0) * Time.deltaTime * 10;
            }
        }
    }

}