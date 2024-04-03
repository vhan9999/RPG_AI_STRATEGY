using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthBar : MonoBehaviour
{

    public GameObject Player;


    public RectTransform HealthBar, Hurt;

    void Update()
    {
        

        HealthBar.sizeDelta = new Vector2(Player.GetComponent<WarriorAgent>().currentHealth, HealthBar.sizeDelta.y);

        if (Hurt.sizeDelta.x > HealthBar.sizeDelta.x)
        {
            Hurt.sizeDelta += new Vector2(-1, 0) * Time.deltaTime * 10;
        }

    }

}