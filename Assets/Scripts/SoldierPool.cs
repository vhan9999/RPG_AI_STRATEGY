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

    private ObjectPool<ClassAgent> rWPool = new ObjectPool<ClassAgent>();
    private ObjectPool<ClassAgent> rBPool = new ObjectPool<ClassAgent>();
    private ObjectPool<ClassAgent> rMPool = new ObjectPool<ClassAgent>();
    private ObjectPool<ClassAgent> rAPool = new ObjectPool<ClassAgent>();

    public GameObject bWarrior;
    public GameObject bBerserker;
    public GameObject bMage;
    public GameObject bArcher;


    public GameObject rWarrior;
    public GameObject rBerserker;
    public GameObject rMage;
    public GameObject rArcher;

    void Awake()
    {
        Debug.Log("init");
        bWPool.InitPool(bWarrior, 10, transform);
        bBPool.InitPool(bBerserker, 10, transform);
        bMPool.InitPool(bMage, 10, transform);
        bAPool.InitPool(bArcher, 10, transform);
        rWPool.InitPool(rWarrior, 10, transform);
        rBPool.InitPool(rBerserker, 10, transform);
        rMPool.InitPool(rMage, 10, transform);
        rAPool.InitPool(rArcher, 10, transform);
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
            default:
                return team == Team.Blue ? bAPool : rAPool;
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
