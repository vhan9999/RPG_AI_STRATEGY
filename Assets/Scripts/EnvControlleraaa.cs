using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;
using System.Linq;

using Random = UnityEngine.Random;

public class EnvControlleraaa : MonoBehaviour
{
    //Max Academy steps before this platform resets
    [Tooltip("Max Environment Steps")] public int MaxEnvironmentSteps = 14000;

    private int[] soldierPrices = { 7, 8, 10 };//w,b,m
    private Profession[] soldierTypes = { Profession.Warrior, Profession.Berserker, Profession.Mage };

    //List of Agents On Platform
    private List<ClassAgent> blueAgentsList = new List<ClassAgent>();
    private List<ClassAgent> redAgentsList = new List<ClassAgent>();

    [SerializeField] private int blueDeadCount = 0;
    [SerializeField] private int redDeadCount = 0;
    [SerializeField] private int blueteamNum = 0;
    [SerializeField] private int redTeamNum = 0;

    [SerializeField]
    private SoldierPool soldierPool;

    private SimpleMultiAgentGroup m_BlueAgentGroup;
    private SimpleMultiAgentGroup m_RedAgentGroup;

    private int m_ResetTimer;



    void Start()
    {
        // Initialize TeamManager
        m_BlueAgentGroup = new SimpleMultiAgentGroup();
        m_RedAgentGroup = new SimpleMultiAgentGroup();

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
        int Count = soldierPrices.Length;
        int money = 100;
        Vector2 SpawnStartPoint = team == Team.Blue ? new Vector2(-18, -23) : new Vector2(-18, 1);
        SimpleMultiAgentGroup m_AgentGroup = team == Team.Blue ? m_BlueAgentGroup : m_RedAgentGroup;
        List<ClassAgent> agentList = team == Team.Blue ? blueAgentsList : redAgentsList;


        agentList.ForEach(a => m_AgentGroup.UnregisterAgent(a));
        agentList.Clear();
        List<int> indexList = Enumerable.Range(0, 228).ToList();
        while(true) 
        {
            //random position
            int randomPos = Random.Range(0, indexList.Count);
            Vector3 spawnPoint = new Vector3(SpawnStartPoint.x + ((indexList[randomPos] % 19) * 2), 1.7f, 
                SpawnStartPoint.y + ((indexList[randomPos] / 19) * 2));//¨M©w¦ì¸m
            indexList.RemoveAt(randomPos);

            //random rotate
            Quaternion randomRot = Quaternion.Euler(0, Random.Range(0, 360), 0);

            //check money enough
            while (Count - 1 >= 0 && soldierPrices[Count - 1] > money)
            {
                Count--;
            }
            if (Count == 0) break;

            //random soldier type
            int soldierPos = Random.Range(0, soldierPrices.Length);
            ClassAgent agent = soldierPool.Spawn(team, soldierTypes[soldierPos], spawnPoint, randomRot, transform.parent);
            money -= soldierPrices[soldierPos];
            agentList.Add(agent);
            m_AgentGroup.RegisterAgent(agent);
            if(team == Team.Blue)
                blueteamNum++;
            else
                redTeamNum++;
        }
    }
}