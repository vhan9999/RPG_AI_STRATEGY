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
    private SimpleMultiAgentGroup m_OrangeAgentGroup;

    private int m_ResetTimer;

    public bool IsRandomScene;

    void Start()
    {
        // Initialize TeamManager
        m_BlueAgentGroup = new SimpleMultiAgentGroup();
        m_OrangeAgentGroup = new SimpleMultiAgentGroup();
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
                    m_OrangeAgentGroup.RegisterAgent(agent);
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
            m_OrangeAgentGroup.GroupEpisodeInterrupted();
            ResetScene();
        }
    }

    public void DeadTouch(Team DeadTeam)
    {
        if (DeadTeam == Team.Blue)
        {
            m_BlueAgentGroup.AddGroupReward(-0.8f);
            m_OrangeAgentGroup.AddGroupReward(1f);
            blueDeadCount++;
        }
        else
        {
            m_OrangeAgentGroup.AddGroupReward(-0.8f);
            m_BlueAgentGroup.AddGroupReward(1f);
            redDeadCount++;
        }
        if (blueDeadCount == teamNum)
        {
            m_OrangeAgentGroup.AddGroupReward(1 * (1 - (float)m_ResetTimer / MaxEnvironmentSteps));
            m_BlueAgentGroup.AddGroupReward(-0.5f * (1 - (float)m_ResetTimer / MaxEnvironmentSteps));
            m_BlueAgentGroup.EndGroupEpisode();
            m_OrangeAgentGroup.EndGroupEpisode();
            ResetScene();
        }
        else if (redDeadCount == teamNum)
        {
            m_OrangeAgentGroup.AddGroupReward(-0.5f * (1 - (float)m_ResetTimer / MaxEnvironmentSteps));
            m_BlueAgentGroup.AddGroupReward(1 * (1 - (float)m_ResetTimer / MaxEnvironmentSteps));
            m_BlueAgentGroup.EndGroupEpisode();
            m_OrangeAgentGroup.EndGroupEpisode();
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
            item.Agent.gameObject.SetActive(true);
            item.Agent.transform.localPosition = item.StartingPos;
            item.Agent.transform.rotation = item.StartingRot;
        }
    }

    private void LoadRandomScene()
    {
        List<int> indexList = Enumerable.Range(0, blueAgentsList.Count).ToList();
        for (int i = 0; i < blueAgentsList.Count; i++)
        {
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