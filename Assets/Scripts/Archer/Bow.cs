using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    [SerializeField]
    private float reloadTime;

    [SerializeField]
    private GameObject arrowPrefab;

    [SerializeField]
    private Transform spawnPoint;

    private bool isReloading;

    [SerializeField]
    public Animator anim;


    [SerializeField]
    public float maxFirePower;

    [SerializeField]
    public float firePowerSpeed;

    public float firePower;

    public bool fire;

    private ClassAgent agent;

    private ObjectPool<Arrow> ArrowPool;

    //public void SetEnemyTag(string enemyTag) {
    //    this.enemyTag = enemyTag;
    //}

    private void Start()
    {
        agent = GetComponentInParent<ClassAgent>();
        ArrowPool = ObjectPool<Arrow>.Instance;
        ArrowPool.InitPool(arrowPrefab, 5);
    }

    public void SetDrawingAnimation(bool value)
    {
        //Debug.Log("Perform Bow Animation");
        anim.SetBool("isReadyFire", value);
    }


    public void Reload() {
        // || currentArrow != null
        if (isReloading) return;
        isReloading = true;
        StartCoroutine(ReloadAfterTime());
    }
    
    private IEnumerator ReloadAfterTime() { 
        yield return new WaitForSeconds(reloadTime);
        isReloading = false;
    }

    public void Fire(float firePower) {
        if (isReloading) return;

        // create arrow prefab
        Arrow currentArrow = ArrowPool.Spawn(spawnPoint.position, transform.rotation);
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
        get => (!isReloading);
    }

    public bool getIsReload {
        get { return isReloading; }
    }
}
