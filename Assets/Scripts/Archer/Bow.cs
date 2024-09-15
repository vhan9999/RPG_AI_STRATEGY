using System.Collections;
using UnityEngine;

public class Bow : MonoBehaviour
{
    [SerializeField]
    private float reloadTime;

    [SerializeField]
    private GameObject arrowPrefab;

    [SerializeField]
    private Transform spawnPoint;

    private ObjectPool<Arrow> arrowPool;
    private ClassAgent agent;
    private bool isReloading;
    public float cooldownTime = 0;

    [SerializeField]
    public Animator anim;

    [SerializeField]
    public float maxFirePower;

    [SerializeField]
    public float firePowerSpeed;

    public float firePower = 10;

    public bool fire = false;

    private void Start()
    {
        arrowPool = ObjectPool<Arrow>.Instance;
        arrowPool.InitPool(arrowPrefab, 2);
        agent = GetComponentInParent<ClassAgent>();
    }
    void Update()
    {
        if (cooldownTime > 0)
            cooldownTime -= Time.deltaTime;
        else
            cooldownTime = 0;
    }
    public void SetDrawingAnimation(bool value)
    {
        anim.SetBool("isReadyFire", value);
    }

    public void Reload()
    {
        isReloading = true;
        Invoke("ReloadAfterTime", reloadTime);
        cooldownTime = reloadTime;
    }

    private void ReloadAfterTime()
    {
        isReloading = false;
    }

    public void Fire(float firePower)
    {
        if (isReloading) return;

        // Get arrow from the pool
        Arrow a = arrowPool.Spawn(agent.transform.position, transform.rotation);
        a.tag = agent.team == Team.Blue ? "BlueArrow" : "RedArrow";
        //Debug.Log(a.tag);
        a.agent = agent;
        
        var force = spawnPoint.TransformVector(Vector3.left * firePower);
        a.Fly(force);
        Reload();
    }


    public bool IsReloading => isReloading;
}
