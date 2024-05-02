using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class EnvController : MonoBehaviour
{
    [System.Serializable]
    public class PlayerInfo
    {
        public ClassAgent Agent;
        [HideInInspector]
        public Vector3 StartingPos;
        [HideInInspector]
        public Quaternion StartingRot;
    }


    /// <summary>
    /// Max Academy steps before this platform resets
    /// </summary>
    /// <returns></returns>
    [Tooltip("Max Environment Steps")] public int MaxEnvironmentSteps = 800000;

    /// <summary>
    /// The area bounds.
    /// </summary>

    /// <summary>
    /// We will be changing the ground material based on success/failue
    /// </summary>

    //List of Agents On Platform
    private List<PlayerInfo> agentsList = new List<PlayerInfo>();

    private int blueDeadCount;
    private int orangeDeadCount;
    private int teamNum;

    private SimpleMultiAgentGroup m_BlueAgentGroup;
    private SimpleMultiAgentGroup m_OrangeAgentGroup;

    private int m_ResetTimer;

    void Start()
    {
        // Initialize TeamManager
        teamNum = 0;
        m_BlueAgentGroup = new SimpleMultiAgentGroup();
        m_OrangeAgentGroup = new SimpleMultiAgentGroup();
        ClassAgent[] agents = GetComponentsInChildren<ClassAgent>();
        foreach (var agent in agents)
        {
            if (agent.enabled)
            {
                agentsList.Add(new PlayerInfo { Agent = agent, StartingPos = agent.transform.position, StartingRot = agent.transform.rotation });
            }
        }
        foreach (var item in agentsList)
        {
            item.StartingPos = item.Agent.transform.localPosition;
            item.StartingRot = item.Agent.transform.rotation;
            if (item.Agent.team == Team.Blue)
            {
                teamNum++;
                m_BlueAgentGroup.RegisterAgent(item.Agent);
            }
            else
            {
                m_OrangeAgentGroup.RegisterAgent(item.Agent);
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
            orangeDeadCount++;
        }
        if (blueDeadCount == teamNum)
        {
            m_OrangeAgentGroup.AddGroupReward(1 * (1 - (float)m_ResetTimer / MaxEnvironmentSteps));
            m_BlueAgentGroup.AddGroupReward(-0.5f * (1 - (float)m_ResetTimer / MaxEnvironmentSteps));
            m_BlueAgentGroup.EndGroupEpisode();
            m_OrangeAgentGroup.EndGroupEpisode();
            ResetScene();
        }
        else if (orangeDeadCount == teamNum)
        {
            m_OrangeAgentGroup.AddGroupReward(-0.5f * (1 - (float)m_ResetTimer / MaxEnvironmentSteps));
            m_BlueAgentGroup.AddGroupReward(1 * (1 - (float)m_ResetTimer / MaxEnvironmentSteps));
            m_BlueAgentGroup.EndGroupEpisode();
            m_OrangeAgentGroup.EndGroupEpisode();
            ResetScene();
        }
    }


    public void ResetScene()
    {
        foreach (var item in agentsList)
        {
            item.Agent.gameObject.SetActive(true);
            item.Agent.transform.localPosition = item.StartingPos;
            item.Agent.transform.rotation = item.StartingRot;
        }
        m_ResetTimer = 0;
        blueDeadCount = 0;
        orangeDeadCount = 0;
    }
}