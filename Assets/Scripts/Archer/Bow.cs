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

    private bool isReloading;

    [SerializeField]
    public Animator anim;

    private Arrow currentArrow;
    [SerializeField]
    public float maxFirePower;

    [SerializeField]
    public float firePowerSpeed;

    public float firePower;

    public bool fire;

    private ClassAgent agent;


    //public void SetEnemyTag(string enemyTag) {
    //    this.enemyTag = enemyTag;
    //}

    private void Start()
    {
        agent = GetComponentInParent<ClassAgent>();
    }

    public void SetDrawingAnimation(bool value)
    {
        //Debug.Log("Perform Bow Animation");
        anim.SetBool("isReadyFire", value);
    }


    public void Reload() {
        // || currentArrow != null
        if (isReloading || currentArrow != null) return;
        isReloading = true;
        StartCoroutine(ReloadAfterTime());
    }
    
    private IEnumerator ReloadAfterTime() { 
        yield return new WaitForSeconds(reloadTime);
        isReloading = false;
    }

    public void Fire(float firePower) {
        if (isReloading || currentArrow != null) return;

        // create arrow prefab
        currentArrow = Instantiate(arrowPrefab, spawnPoint);
        currentArrow.agent = agent;
        currentArrow.transform.localPosition = Vector3.zero;
        // tag decision
        currentArrow.tag = agent.team == Team.Blue ? "BlueArrow" : "RedArrow";
        
        var force = spawnPoint.TransformVector(Vector3.left * firePower);
        currentArrow.Fly(force);
        currentArrow = null;
        Reload();
    }

    // change to getter 
    public bool isReady {
        //  && currentArrow != null
        get => (!isReloading || currentArrow != null);
    }

    public bool getIsReload {
        get { return isReloading; }
    }
}
