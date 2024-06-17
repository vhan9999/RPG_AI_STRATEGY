using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class SoldierPool : MonoBehaviour
{
    private ObjectPool<WarriorAgent> bWPool = new ObjectPool<WarriorAgent>();
    private ObjectPool<BerserkerAgent> bBPool = new ObjectPool<BerserkerAgent>();
    private ObjectPool<MageAgent> bMPool = new ObjectPool<MageAgent>();

    private ObjectPool<WarriorAgent> rWPool = new ObjectPool<WarriorAgent>();
    private ObjectPool<BerserkerAgent> rBPool = new ObjectPool<BerserkerAgent>();
    private ObjectPool<MageAgent> rMPool = new ObjectPool<MageAgent>();

    public GameObject bWarrior;
    public GameObject bBerserker;
    public GameObject bMage;

    public GameObject rWarrior;
    public GameObject rBerserker;
    public GameObject rMage;

    void Awake()
    {
        //bWPool = new ObjectPool<WarriorAgent>();
        //bBPool = new ObjectPool<BerserkerAgent>();
        //bMPool = new ObjectPool<MageAgent>();
        //rWPool = new ObjectPool<WarriorAgent>();
        //rBPool = new ObjectPool<BerserkerAgent>();
        //rMPool = new ObjectPool<MageAgent>();

        bWPool.InitPool(bWarrior, 10, transform);
        bBPool.InitPool(bBerserker, 10, transform);
        bMPool.InitPool(bMage, 10, transform);
        rWPool.InitPool(bWarrior, 10, transform);
        rBPool.InitPool(bBerserker, 10, transform);
        rMPool.InitPool(bMage, 10, transform);
    }

    public void Spawn(Team team, Profession profession, Vector3 position, Quaternion quaternion)
    {
        if (team == Team.Blue)
        {
            switch (profession)
            {
                case Profession.Warrior:
                    bWPool.Spawn(position, quaternion);
                    break;
                case Profession.Berserker:
                    bBPool.Spawn(position, quaternion);
                    break;
                case Profession.Mage:
                    bMPool.Spawn(position, quaternion);
                    break;
            }
        }
        else
        {
            switch (profession)
            {
                case Profession.Warrior:
                    rWPool.Spawn(position, quaternion);
                    break;
                case Profession.Berserker:
                    rBPool.Spawn(position, quaternion);
                    break;
                case Profession.Mage:
                    rMPool.Spawn(position, quaternion);
                    break;
            }
        }
    }
}
