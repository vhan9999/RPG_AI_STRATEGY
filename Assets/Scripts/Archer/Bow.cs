using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bow : MonoBehaviour
{
    [HideInInspector]
    public bool IsAttack = false;
    public bool IsReload = false;
    private ArrowShoot arrowShoot;

    [SerializeField] private GameObject arrow;

    private ObjectPool<Arrow> arrowPool;
    private ClassAgent agent;

    private void Start()
    {
        arrowShoot = transform.GetChild(0).GetComponentInChildren<ArrowShoot>();
        arrowPool = ObjectPool<Arrow>.Instance;
        arrowPool.InitPool(arrow, 30);
        agent = GetComponentInParent<ClassAgent>();
    }

    private void Update()
    {
        
    }

    public void AttackCast() {
        if (!IsAttack) {
            IsAttack = true;
            Invoke("AttackShoot", 0.8f);
        }
    }

    public void AttackShoot() {
        Arrow a = arrowPool.Spawn(transform.position + transform.up, transform.rotation);
        a.tag = agent.team == Team.Blue ? "BlueArrow" : "RedArrow";
        a.moveDir = transform.forward;
        a.agent = agent;
        a.Reset();
        IsAttack = false;
    }

    public void ArrowShooted()
    {
        if (!IsAttack && !IsReload)
        {
            arrowShoot.CastStart();
            IsReload = true;
            Invoke("Reload", 15f);
        }
    }

    public void Reload() { 
        IsReload = false;
    }

    //public void SetDrawingAnimation(bool value)
    //{
    //    //Debug.Log("Perform Bow Animation");
    //    anim.SetBool("isReadyFire", value);
    //}


    //public void Reload() {
    //    // || currentArrow != null
    //    if (isReloading || currentArrow != null) return;
    //    isReloading = true;
    //    StartCoroutine(ReloadAfterTime());
    //}

    //private IEnumerator ReloadAfterTime() { 
    //    yield return new WaitForSeconds(reloadTime);
    //    isReloading = false;
    //}

    //public void Fire(float firePower) {
    //    if (isReloading || currentArrow != null) return;

    //    // create arrow prefab
    //    currentArrow = Instantiate(arrowPrefab, spawnPoint);
    //    currentArrow.agent = agent;
    //    currentArrow.transform.localPosition = Vector3.zero;
    //    // tag decision
    //    currentArrow.tag = agent.team == Team.Blue ? "BlueArrow" : "RedArrow";

    //    var force = spawnPoint.TransformVector(Vector3.left * firePower);
    //    currentArrow.Fly(force);
    //    currentArrow = null;
    //    Reload();
    //}

    //// change to getter 
    //public bool isReady {
    //    //  && currentArrow != null
    //    get => (!isReloading || currentArrow != null);
    //}

    //public bool getIsReload {
    //    get { return isReloading; }
    //}
}
