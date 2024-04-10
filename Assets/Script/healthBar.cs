using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{

    public GameObject Player;


    public RectTransform Health, Hurt;

    void Update()
    {
        

        Health.sizeDelta = new Vector2(Player.GetComponent<WarriorAgent>().currentHealth*2, Health.sizeDelta.y);

        if (Hurt.sizeDelta.x > Health.sizeDelta.x)
        {
            Hurt.sizeDelta += new Vector2(-1, 0) * Time.deltaTime * 10;
        }

    }

}