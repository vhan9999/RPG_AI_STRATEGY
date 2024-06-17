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

    private ObjectPool<ClassAgent> rWPool = new ObjectPool<ClassAgent>();
    private ObjectPool<ClassAgent> rBPool = new ObjectPool<ClassAgent>();
    private ObjectPool<ClassAgent> rMPool = new ObjectPool<ClassAgent>();

    public GameObject bWarrior;
    public GameObject bBerserker;
    public GameObject bMage;

    public GameObject rWarrior;
    public GameObject rBerserker;
    public GameObject rMage;

    void Awake()
    {
        bWPool.InitPool(bWarrior, 10, transform);
        bBPool.InitPool(bBerserker, 10, transform);
        bMPool.InitPool(bMage, 10, transform);
        rWPool.InitPool(bWarrior, 10, transform);
        rBPool.InitPool(bBerserker, 10, transform);
        rMPool.InitPool(bMage, 10, transform);
    }

    private ObjectPool<ClassAgent> GetPool(Team team, Profession profession)
    {
        switch (profession)
        {
            case Profession.Warrior:
                return team == Team.Blue ? bWPool : rWPool;
            case Profession.Berserker:
                return team == Team.Blue ? bBPool : rBPool;
            default:
                return team == Team.Blue ? bMPool : rMPool;
        }
    }

    public ClassAgent Spawn(Team team, Profession profession, Vector3 position, Quaternion quaternion, Transform transform)
    {
        ObjectPool<ClassAgent> pool = GetPool(team, profession);
        return pool.Spawn(position, quaternion, transform);
    }

    public void Rycle(Team team, Profession profession, ClassAgent obj)
    {
        ObjectPool<ClassAgent> pool = GetPool(team, profession);
        pool.Recycle(obj);
    }
}
