using System.Collections.Generic;
using Unity.MLAgents;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using System;
using static UnityEditor.Progress;
using System.Linq;

using Random = UnityEngine.Random;

public class EnvControlleraaa : MonoBehaviour
{
    [Serializable]
    public class PlayerInfo
    {
        public ClassAgent Agent;
        [HideInInspector]
        public Vector3 StartingPos;
        [HideInInspector]
        public Quaternion StartingRot;
    }

    //Max Academy steps before this platform resets
    [Tooltip("Max Environment Steps")] public int MaxEnvironmentSteps = 14000;

    private List<int> soldierPrices = new List<int> { 7, 8, 10 };//w,b,m

    //List of Agents On Platform
    private List<ClassAgent> blueAgentsList = new List<ClassAgent>();
    private List<ClassAgent> redAgentsList = new List<ClassAgent>();

    [SerializeField] private int blueDeadCount = 0;
    [SerializeField] private int redDeadCount = 0;
    [SerializeField] private int blueteamNum = 0;
    [SerializeField] private int redTeamNum = 0;

    private SimpleMultiAgentGroup m_BlueAgentGroup;
    private SimpleMultiAgentGroup m_RedAgentGroup;

    private int m_ResetTimer;

    private ObjectPool<WarriorAgent> blueWarriorPool = new ObjectPool<WarriorAgent> ();
    private ObjectPool<WarriorAgent> redWarriorPool = new ObjectPool<WarriorAgent>();
    private ObjectPool<BerserkerAgent> blueBerserkerPool = new ObjectPool<BerserkerAgent>();
    private ObjectPool<BerserkerAgent> redBerserkerPool = new ObjectPool<BerserkerAgent>();
    private ObjectPool<MageAgent> blueMagePool = new ObjectPool<MageAgent>();
    private ObjectPool<MageAgent> redMagePool = new ObjectPool<MageAgent>();

    [SerializeField] private GameObject blueWarrior;
    [SerializeField] private GameObject redWarrior;
    [SerializeField] private GameObject blueBerserker;
    [SerializeField] private GameObject redBerserker;
    [SerializeField] private GameObject blueMage;
    [SerializeField] private GameObject redMage;



    void Start()
    {
        // Initialize TeamManager
        m_BlueAgentGroup = new SimpleMultiAgentGroup();
        m_RedAgentGroup = new SimpleMultiAgentGroup();
        blueWarriorPool.InitPool(blueWarrior, 14, transform);
        redWarriorPool.InitPool(redWarrior, 14, transform);
        blueBerserkerPool.InitPool(blueBerserker, 12, transform);
        redBerserkerPool.InitPool(redBerserker, 12, transform);
        blueMagePool.InitPool(blueMage, 10, transform);
        redMagePool.InitPool(redMage, 10, transform);
        ResetScene();
    }

    void FixedUpdate()
    {
        m_ResetTimer += 1;
        if (m_ResetTimer >= MaxEnvironmentSteps && MaxEnvironmentSteps > 0)
        {
            Debug.Log("times up");
            m_BlueAgentGroup.GroupEpisodeInterrupted();
            m_RedAgentGroup.GroupEpisodeInterrupted();
            ResetScene();
        }
    }

    public void DeadTouch(Team DeadTeam)
    {
        
        if (DeadTeam == Team.Blue)
        {
            blueDeadCount++;
            
        }
        else
        {
            redDeadCount++;

        }
        if (blueDeadCount == blueteamNum)
        {
            Debug.Log("red win"+blueDeadCount+" "+ blueteamNum);
            m_BlueAgentGroup.AddGroupReward(-(1 - m_ResetTimer / MaxEnvironmentSteps));
            m_RedAgentGroup.AddGroupReward(1);
            m_BlueAgentGroup.EndGroupEpisode();
            m_RedAgentGroup.EndGroupEpisode();
            ResetScene();
            
        }
        else if (redDeadCount == redTeamNum)
        {
            Debug.Log("blue win" + redDeadCount + " " + redTeamNum);
            m_BlueAgentGroup.AddGroupReward(1);
            m_RedAgentGroup.AddGroupReward(-(1 - m_ResetTimer / MaxEnvironmentSteps));
            m_BlueAgentGroup.EndGroupEpisode();
            m_RedAgentGroup.EndGroupEpisode();
            ResetScene();
        }
    }

    private void ResetScene()
    {
        m_ResetTimer = 0;
        blueDeadCount = 0;
        redDeadCount = 0;
        blueteamNum = 0;
        redTeamNum = 0;
        LoadRandomScene(Team.Blue);
        LoadRandomScene(Team.Red);

        
    }

    private void LoadRandomScene(Team team)
    {
        ClassAgent agent = null;
        int Count = soldierPrices.Count;
        int Money = 100;
        Vector2 SpawnStartPoint = team == Team.Blue ? new Vector2(-18, -23) : new Vector2(-18, 1);
        ObjectPool<WarriorAgent> warriorPool = team == Team.Blue ? blueWarriorPool : redWarriorPool;
        ObjectPool<BerserkerAgent> berserkerPool = team == Team.Blue ? blueBerserkerPool : redBerserkerPool;
        ObjectPool<MageAgent> magePool = team == Team.Blue ? blueMagePool : redMagePool;
        SimpleMultiAgentGroup m_AgentGroup = team == Team.Blue ? m_BlueAgentGroup : m_RedAgentGroup;
        List<ClassAgent> agentList = team == Team.Blue ? blueAgentsList : redAgentsList;
        

        foreach (ClassAgent a in agentList)
        {
            m_AgentGroup.UnregisterAgent(a);
            if (a is WarriorAgent)
                warriorPool.Recycle((WarriorAgent)a);
            else if (a is BerserkerAgent)
                berserkerPool.Recycle((BerserkerAgent)a);
            else if(a is MageAgent)
                magePool.Recycle((MageAgent)a);
        }
        agentList.Clear();
        List<int> indexList = Enumerable.Range(0, 228).ToList();
        while(true) 
        {
            //random position
            int randomPos = Random.Range(0, indexList.Count);
            Vector3 spawnPoint = new Vector3(SpawnStartPoint.x + ((indexList[randomPos] % 19) * 2), 1.7f, SpawnStartPoint.y + ((indexList[randomPos] / 19) * 2));//¨M©w¦ì¸m
            indexList.RemoveAt(randomPos);

            //random rotate
            int randomRot = Random.Range(0, 360);

            //check money enough
            while(Count - 1 >= 0 && soldierPrices[Count - 1] > Money)
            {
                Count--;
            }
            if (Count == 0)
                break;
            //random soldier type
            int randomSoldier = Random.Range(0, soldierPrices.Count);
            if (randomSoldier == 0)
            {
                agent = warriorPool.Spawn( transform.position + spawnPoint, Quaternion.Euler(new Vector3(0, randomRot, 0)), transform);
            }
            else if (randomSoldier == 1)
            {
                agent = berserkerPool.Spawn(transform.position + spawnPoint, Quaternion.Euler(new Vector3(0, randomRot, 0)), transform);
            }
            else if (randomSoldier == 2)
            {
                agent = magePool.Spawn(transform.position + spawnPoint, Quaternion.Euler(new Vector3(0, randomRot, 0)), transform);
            }
            Money -= soldierPrices[randomSoldier];
            agentList.Add(agent);
            m_AgentGroup.RegisterAgent(agent);
            if(team == Team.Blue)
                blueteamNum++;
            else
                redTeamNum++;
        }

        //red
    }
}