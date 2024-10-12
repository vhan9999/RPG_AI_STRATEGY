using System.Collections.Generic;
using Unity.MLAgents;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using System;
using static UnityEditor.Progress;
using System.Linq;

using Random = UnityEngine.Random;
using Unity.VisualScripting;

public class EnvController : MonoBehaviour, IEnvController
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
    [Tooltip("Max Environment Steps")] private int MaxEnvironmentSteps = 5000;

    //List of Agents On Platform
    private List<PlayerInfo> blueAgentsList = new List<PlayerInfo>();
    private List<PlayerInfo> redAgentsList = new List<PlayerInfo>();

    public int blueDeadCount = 0;
    public int redDeadCount = 0;
    public int teamNum = 0;

    private SimpleMultiAgentGroup m_BlueAgentGroup;
    private SimpleMultiAgentGroup m_RedAgentGroup;
    private TankAgent redTank;
    private TankAgent blueTank;

    private int m_ResetTimer;

    public bool IsRandomScene;

    void Start()
    {
        // Initialize TeamManager
        m_BlueAgentGroup = new SimpleMultiAgentGroup();
        m_RedAgentGroup = new SimpleMultiAgentGroup();
        ClassAgent[] agents = GetComponentsInChildren<ClassAgent>();
        foreach (ClassAgent agent in agents)
        {
            if (agent.enabled)
            {
                if (agent.team == Team.Blue)
                {
                    if (agent is TankAgent) blueTank = (TankAgent)agent;
                    blueAgentsList.Add(new PlayerInfo { Agent = agent, StartingPos = agent.transform.localPosition, StartingRot = agent.transform.rotation });
                    teamNum++;
                    m_BlueAgentGroup.RegisterAgent(agent);
                }
                else
                {
                    if (agent is TankAgent) redTank = (TankAgent)agent;
                    redAgentsList.Add(new PlayerInfo { Agent = agent, StartingPos = agent.transform.localPosition, StartingRot = agent.transform.rotation });
                    m_RedAgentGroup.RegisterAgent(agent);
                }
            }
        }
        ResetScene();
    }

    void FixedUpdate()
    {
        m_ResetTimer += 1;
        if (m_ResetTimer >= MaxEnvironmentSteps && MaxEnvironmentSteps > 0)
        {
            
            if (!GameArgs.IsDense) m_RedAgentGroup.AddGroupReward(-0.5f);
            if (!GameArgs.IsDense) m_BlueAgentGroup.AddGroupReward(-0.5f);
            ResetScene();
            m_BlueAgentGroup.GroupEpisodeInterrupted();
            m_RedAgentGroup.GroupEpisodeInterrupted();
        }
    }

    public void DeadTouch(Team DeadTeam)
    {
        if (DeadTeam == Team.Blue)
        {
            if (!GameArgs.IsDense) m_RedAgentGroup.AddGroupReward(1f / teamNum);
            if (!GameArgs.IsDense) m_BlueAgentGroup.AddGroupReward(-(1f / teamNum));
            blueDeadCount++;
        }
        else
        {
            if (!GameArgs.IsDense) m_BlueAgentGroup.AddGroupReward(1f / teamNum);
            if (!GameArgs.IsDense) m_RedAgentGroup.AddGroupReward(-(1f / teamNum));
            redDeadCount++;
        }
        if (blueDeadCount == teamNum)
        {
            if (GameArgs.IsDense)
            {
                m_BlueAgentGroup.AddGroupReward(-0.7f * (2-GameArgs.rewardRatio));
                m_RedAgentGroup.AddGroupReward(2 - GameArgs.rewardRatio);
            }
            ResetScene();
            m_BlueAgentGroup.EndGroupEpisode();
            m_RedAgentGroup.EndGroupEpisode();
        }
        else if (redDeadCount == teamNum)
        {
            if (GameArgs.IsDense)
            {
                Debug.Log("BlueWin");
                m_BlueAgentGroup.AddGroupReward(2 - GameArgs.rewardRatio);
                m_RedAgentGroup.AddGroupReward(-0.7f * (2 - GameArgs.rewardRatio));
            }
            ResetScene();
            m_BlueAgentGroup.EndGroupEpisode();
            m_RedAgentGroup.EndGroupEpisode();
        }
    }

    private void ResetScene()
    {
        if (IsRandomScene)
        {
            LoadRandomScene();
        }
        else
        {
            LoadFixedScene();
        }

        //reward
        if (GameArgs.rewardRatio > 0.3f)
        {
            GameArgs.rewardRatio -= 0.000035f;
        }

        m_ResetTimer = 0;
        blueDeadCount = 0;
        redDeadCount = 0;

    }

    private void LoadFixedScene()
    {
        foreach (PlayerInfo item in blueAgentsList.Concat(redAgentsList))
        {
            Debug.Log("LoadFixedScene");
            if (item.Agent.gameObject.activeSelf) item.Agent.GameOver();
            item.Agent.transform.localPosition = item.StartingPos;
            item.Agent.transform.rotation = item.StartingRot;
        }
    }

    private void LoadRandomScene()
    {
        List<int> blueIndexList = Enumerable.Range(0, blueAgentsList.Count).ToList();
        List<int> redIndexList = Enumerable.Range(0, redAgentsList.Count).ToList();

        foreach (PlayerInfo item in blueAgentsList)
        {
            if (item.Agent.gameObject.activeSelf) item.Agent.GameOver();
            item.Agent.gameObject.SetActive(true);
            int randomNum = Random.Range(0, blueIndexList.Count);
            int pos = blueIndexList[randomNum];
            blueIndexList.RemoveAt(randomNum);
            item.Agent.transform.localPosition = blueAgentsList[pos].StartingPos;
            item.Agent.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        }

        foreach (PlayerInfo item in redAgentsList)
        {
            if (item.Agent.gameObject.activeSelf) item.Agent.GameOver();
            item.Agent.gameObject.SetActive(true);
            int randomNum = Random.Range(0, redIndexList.Count);
            int pos = redIndexList[randomNum];
            redIndexList.RemoveAt(randomNum);
            item.Agent.transform.localPosition = redAgentsList[pos].StartingPos;
            item.Agent.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        }
    }

    private ClassAgent findMostCloseEnemy(ClassAgent agent)
    {
        List<PlayerInfo> agentList = agent.team == Team.Blue ? redAgentsList : blueAgentsList;

        ClassAgent mostCloseAgent = agentList[0].Agent;
        float currentDistance = Vector3.Distance(agent.transform.position, mostCloseAgent.transform.position);
        for (int i = 1; i < agentList.Count; i++)
        {
            float distance = Vector3.Distance(agent.transform.position, agentList[i].Agent.transform.position);
            if (currentDistance > distance)
            {
                mostCloseAgent = agentList[i].Agent;
                currentDistance = distance;
            }
        }

        return mostCloseAgent;
    }

    public void DistanceReward(ClassAgent agent, float reward)
    {
        ClassAgent mostCloseAgent = findMostCloseEnemy(agent);
        float distance = Vector3.Distance(agent.transform.position, mostCloseAgent.transform.position);

        if (distance > 8)
        {
            agent.AddReward(reward * 2f);
        }
        if (distance > 4)
        {
            agent.AddReward(reward);
        }
    }

    public void tankPenalty(Team team, float teammatePenalty)
    {
        if(GameArgs.IsDense)(team == Team.Red ? redTank : blueTank)?.AddReward(teammatePenalty/4);
    }
}

