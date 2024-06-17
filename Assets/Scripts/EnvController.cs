using System.Collections.Generic;
using Unity.MLAgents;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using System;
using static UnityEditor.Progress;
using System.Linq;

using Random = UnityEngine.Random;

public class EnvController : MonoBehaviour
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
    [Tooltip("Max Environment Steps")] public int MaxEnvironmentSteps = 20000;

    //List of Agents On Platform
    private List<PlayerInfo> blueAgentsList = new List<PlayerInfo>();
    private List<PlayerInfo> redAgentsList = new List<PlayerInfo>();

    private int blueDeadCount = 0;
    private int redDeadCount = 0;
    private int teamNum = 0;

    private SimpleMultiAgentGroup m_BlueAgentGroup;
    private SimpleMultiAgentGroup m_RedAgentGroup;

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
                    blueAgentsList.Add(new PlayerInfo { Agent = agent, StartingPos = agent.transform.localPosition, StartingRot = agent.transform.rotation });
                    teamNum++;
                    m_BlueAgentGroup.RegisterAgent(agent);
                }
                else
                {
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
        if (blueDeadCount == teamNum)
        {
            m_BlueAgentGroup.AddGroupReward(-(1 - m_ResetTimer / MaxEnvironmentSteps));
            m_RedAgentGroup.AddGroupReward(1);
            m_BlueAgentGroup.EndGroupEpisode();
            m_RedAgentGroup.EndGroupEpisode();
            ResetScene();
        }
        else if (redDeadCount == teamNum)
        {
            m_BlueAgentGroup.AddGroupReward(1);
            m_RedAgentGroup.AddGroupReward(-(1 - m_ResetTimer / MaxEnvironmentSteps));
            m_BlueAgentGroup.EndGroupEpisode();
            m_RedAgentGroup.EndGroupEpisode();
            ResetScene();
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
        m_ResetTimer = 0;
        blueDeadCount = 0;
        redDeadCount = 0;
    }

    private void LoadFixedScene()
    {
        foreach (PlayerInfo item in blueAgentsList.Concat(redAgentsList))
        {
            item.Agent.gameObject.SetActive(false);
            item.Agent.gameObject.SetActive(true);
            item.Agent.transform.localPosition = item.StartingPos;
            item.Agent.transform.rotation = item.StartingRot;
        }
    }

    private void LoadRandomScene()
    {
        
        int count = Mathf.Min(blueAgentsList.Count, redAgentsList.Count);
        List<int> indexList = Enumerable.Range(0, count).ToList();
        for (int i = 0; i < count; i++)
        {
            blueAgentsList[i].Agent.gameObject.SetActive(false);
            redAgentsList[i].Agent.gameObject.SetActive(false);
            blueAgentsList[i].Agent.gameObject.SetActive(true);
            redAgentsList[i].Agent.gameObject.SetActive(true);
            int randomNum = Random.Range(0, indexList.Count);
            int pos = indexList[randomNum];
            blueAgentsList[i].Agent.transform.localPosition = blueAgentsList[pos].StartingPos;
            blueAgentsList[i].Agent.transform.rotation = blueAgentsList[pos].StartingRot;
            redAgentsList[i].Agent.transform.localPosition = redAgentsList[pos].StartingPos;
            redAgentsList[i].Agent.transform.rotation = redAgentsList[pos].StartingRot;
            indexList.RemoveAt(randomNum);
        }
    }
}