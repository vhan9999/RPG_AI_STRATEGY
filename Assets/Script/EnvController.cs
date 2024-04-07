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
        [HideInInspector]
        public Rigidbody Rb;
    }


    /// <summary>
    /// Max Academy steps before this platform resets
    /// </summary>
    /// <returns></returns>
    [Tooltip("Max Environment Steps")] public int MaxEnvironmentSteps = 25000;

    /// <summary>
    /// The area bounds.
    /// </summary>

    /// <summary>
    /// We will be changing the ground material based on success/failue
    /// </summary>

    //List of Agents On Platform
    public List<PlayerInfo> AgentsList = new List<PlayerInfo>();

    private int blueIsDead;
    private int orangeIsDead;
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
        foreach (var item in AgentsList)
        {
            item.StartingPos = item.Agent.transform.position;
            item.StartingRot = item.Agent.transform.rotation;
            item.Rb = item.Agent.GetComponent<Rigidbody>();
            if (item.Agent.team == Team.Blue)
            {
                m_BlueAgentGroup.RegisterAgent(item.Agent);
                teamNum++;
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
            m_BlueAgentGroup.AddGroupReward(-1);
            m_OrangeAgentGroup.AddGroupReward(1);
            blueIsDead++;
        }
        else
        {
            m_OrangeAgentGroup.AddGroupReward(-1);
            m_BlueAgentGroup.AddGroupReward(1);
            orangeIsDead++;
        }
        if(blueIsDead == teamNum)
        {
            m_OrangeAgentGroup.AddGroupReward(5);
            m_OrangeAgentGroup.EndGroupEpisode();
            m_BlueAgentGroup.EndGroupEpisode();
            ResetScene();
        }
        if (orangeIsDead == teamNum)
        {
            m_BlueAgentGroup.AddGroupReward(5);
            m_OrangeAgentGroup.EndGroupEpisode();
            m_BlueAgentGroup.EndGroupEpisode();
            ResetScene();
        }

        

    }


    public void ResetScene()
    {
        m_ResetTimer = 0;
        blueIsDead = 0;
        orangeIsDead = 0;
    }
}