using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class SoldierPool : MonoBehaviour
{
    private ObjectPool<ClassAgent> bWPool = new ObjectPool<ClassAgent>();
    private ObjectPool<ClassAgent> bBPool = new ObjectPool<ClassAgent>();
    private ObjectPool<ClassAgent> bMPool = new ObjectPool<ClassAgent>();
    private ObjectPool<ClassAgent> bAPool = new ObjectPool<ClassAgent>();
    private ObjectPool<ClassAgent> bTPool = new ObjectPool<ClassAgent>();

    private ObjectPool<ClassAgent> rWPool = new ObjectPool<ClassAgent>();
    private ObjectPool<ClassAgent> rBPool = new ObjectPool<ClassAgent>();
    private ObjectPool<ClassAgent> rMPool = new ObjectPool<ClassAgent>();
    private ObjectPool<ClassAgent> rAPool = new ObjectPool<ClassAgent>();
    private ObjectPool<ClassAgent> rTPool = new ObjectPool<ClassAgent>();

    public GameObject bWarrior;
    public GameObject bBerserker;
    public GameObject bMage;
    public GameObject bArcher;
    public GameObject bTank;

    public GameObject rWarrior;
    public GameObject rBerserker;
    public GameObject rMage;
    public GameObject rArcher;
    public GameObject rTank;

    void Awake()
    {
        Debug.Log("init");
        bWPool.InitPool(bWarrior, 5, transform);
        bBPool.InitPool(bBerserker, 5, transform);
        bMPool.InitPool(bMage, 5, transform);
        bAPool.InitPool(bArcher, 5, transform);
        bTPool.InitPool(bTank, 5, transform);
        rWPool.InitPool(rWarrior, 5, transform);
        rBPool.InitPool(rBerserker, 5, transform);
        rMPool.InitPool(rMage, 5, transform);
        rAPool.InitPool(rArcher, 5, transform);
        rTPool.InitPool(rTank, 5, transform);
    }

    private ObjectPool<ClassAgent> GetPool(Team team, Profession profession)
    {
        switch (profession)
        {
            case Profession.Warrior:
                return team == Team.Blue ? bWPool : rWPool;
            case Profession.Berserker:
                return team == Team.Blue ? bBPool : rBPool;
            case Profession.Mage:
                return team == Team.Blue ? bMPool : rMPool;
            case Profession.Archer:
                return team == Team.Blue ? bAPool : rAPool;
            case Profession.Tank:
                return team == Team.Blue ? bTPool : rTPool;
            default: return null;
        }
    }

    public ClassAgent Spawn(Team team, Profession profession, Vector3 position, Quaternion quaternion, Transform parent = null)
    {
        ObjectPool<ClassAgent> pool = GetPool(team, profession);
        return pool.Spawn(position, quaternion, parent);
    }

    public void Rycle(Team team, Profession profession, ClassAgent obj)
    {
        ObjectPool<ClassAgent> pool = GetPool(team, profession);
        pool.Recycle(obj);
    }
}
