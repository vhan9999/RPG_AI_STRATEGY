using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArcherAgent : ClassAgent
{
    [SerializeField]
    private Bow bow;

    [SerializeField]
    private string enemyTag;

    [SerializeField]
    private float maxFirePower;

    [SerializeField]
    private float firePowerSpeed;

    private float firePower;

    private bool fire;

    private void Start()
    {
        bow.SetEnemyTag(enemyTag);
        bow.Reload();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) { 
            fire = true;
        }
        if (fire && firePower < maxFirePower) {
            firePower += Time.deltaTime * firePowerSpeed;
        }
        if (fire && Input.GetMouseButtonUp(0)) {
            bow.Fire(firePower);
            firePower = 0;
            fire = false;
        }
    }
}
