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

    [SerializeField]
    public Animator anim;

    [SerializeField]
    public float maxFirePower;

    [SerializeField]
    public float firePowerSpeed;

    public float firePower;

    public bool fire;

    private void Start()
    {
        arrowPool = ObjectPool<Arrow>.Instance;
        arrowPool.InitPool(arrowPrefab, 2);
        agent = GetComponentInParent<ClassAgent>();
    }

    public void SetDrawingAnimation(bool value)
    {
        anim.SetBool("isReadyFire", value);
    }

    public void Reload()
    {
        isReloading = true;
        Invoke("ReloadAfterTime", reloadTime);
    }

    private void ReloadAfterTime()
    {
        isReloading = false;
    }

    public void Fire(float firePower)
    {
        if (isReloading) return;

        // Get arrow from the pool
        agent.AddReward(-0.03f);
        Arrow a = arrowPool.Spawn(spawnPoint.position, transform.rotation);
        a.tag = agent.team == Team.Blue ? "BlueArrow" : "RedArrow";
        Debug.Log(a.tag);
        a.agent = agent;
        
        var force = spawnPoint.TransformVector(Vector3.left * firePower);
        a.Fly(force);
        Reload();
    }


    public bool IsReloading => isReloading;
}
