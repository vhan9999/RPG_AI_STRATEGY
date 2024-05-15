using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    [SerializeField]
    private float reloadTime;

    [SerializeField]
    private Arrow arrowPrefab;

    [SerializeField]
    private Transform spawnPoint;

    private Arrow currentArrow;
    private string enemyTag;
    private bool isReloading;

    [SerializeField]
    public Animator anim;


    [SerializeField]
    public float maxFirePower;

    [SerializeField]
    public float firePowerSpeed;

    public float firePower;

    public bool fire;


    public void SetEnemyTag(string enemyTag) {
        this.enemyTag = enemyTag;
    }

    public void SetDrawingAnimation(bool value)
    {
        //Debug.Log("Perform Bow Animation");
        anim.SetBool("isReadyFire", value);
    }


    public void Reload() {
        if (isReloading || currentArrow != null) return;
        isReloading = true;
        StartCoroutine(ReloadAfterTime());
    }
    
    private IEnumerator ReloadAfterTime() { 
        yield return new WaitForSeconds(reloadTime);
        isReloading = false;
    }

    public void Fire(float firePower) {
        if (isReloading && currentArrow == null) return;

        // create arrow prefab
        currentArrow = Instantiate(arrowPrefab, spawnPoint);
        currentArrow.transform.localPosition = Vector3.zero;
        currentArrow.SetEnemyTag(enemyTag);

        var force = spawnPoint.TransformVector(Vector3.left * firePower);
        currentArrow.Fly(force);
        currentArrow = null;
        Reload();
    }

    // change to getter 
    public bool isReady {
        get => (!isReloading && currentArrow != null);
    }

    public bool getIsReload {
        get { return isReloading; }
    }
}
