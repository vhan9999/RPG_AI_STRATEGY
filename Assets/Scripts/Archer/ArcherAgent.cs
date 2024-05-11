using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Policies;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class ArcherAgent : ClassAgent
{
    [SerializeField]
    private Bow bow;

    [SerializeField]
    private string enemyTag;

    private void Start()
    {
        bow = GetComponentInChildren<Bow>();
        bow.SetEnemyTag(enemyTag);
        bow.Reload();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            bow.fire = true;
            bow.SetDrawingAnimation(true); // start drawing animation
        }
        if (bow.fire && bow.firePower < bow.maxFirePower)
        {
            bow.firePower += Time.deltaTime * bow.firePowerSpeed;
        }
        if (bow.fire && Input.GetMouseButtonUp(0))
        {
            bow.Fire(bow.firePower);
            bow.firePower = 0;
            bow.fire = false;
            bow.SetDrawingAnimation(false); // Stop drawing animation
        }
    }
}
