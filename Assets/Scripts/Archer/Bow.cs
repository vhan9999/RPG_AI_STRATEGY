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

    private void Start()
    {
        arrowPool = ObjectPool<Arrow>.Instance;
        arrowPool.InitPool(arrowPrefab, 5);
    }

    public void SetEnemyTag(string enemyTag)
    {
        this.enemyTag = enemyTag;
    }

    public void SetDrawingAnimation(bool value)
    {
        anim.SetBool("isReadyFire", value);
    }

    public void Reload()
    {
        if (isReloading || currentArrow != null) return;
        isReloading = true;
        StartCoroutine(ReloadAfterTime());
    }

    private IEnumerator ReloadAfterTime()
    {
        yield return new WaitForSeconds(reloadTime);
        isReloading = false;
    }

    public void Fire(float firePower)
    {
        if (isReloading && currentArrow == null) return;

        // Get arrow from the pool
        currentArrow = arrowPool.Spawn(spawnPoint.position, spawnPoint.rotation);
        currentArrow.SetEnemyTag(enemyTag);

        var force = spawnPoint.TransformVector(Vector3.left * firePower);
        currentArrow.Fly(force);
        currentArrow = null;
        Reload();
    }

    public bool isReady => (!isReloading && currentArrow != null);

    public bool IsReloading => isReloading;
}
