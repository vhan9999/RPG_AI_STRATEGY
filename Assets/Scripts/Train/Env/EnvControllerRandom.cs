using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;
using System.Linq;

using Random = UnityEngine.Random;
using UnityEngine.UIElements;
using System;

public class EnvControllerRandom : MonoBehaviour, IEnvController
{
    //Max Academy steps before this platform resets
    [Tooltip("Max Environment Steps")] public int MaxEnvironmentSteps = 5000;

    private Profession[] soldierTypes = { Profession.Warrior, Profession.Berserker, Profession.Mage, Profession.Archer, Profession.Tank };

    //List of Agents On Platform
    private List<ClassAgent> blueAgentsList = new List<ClassAgent>();
    private List<ClassAgent> redAgentsList = new List<ClassAgent>();
    private List<TankAgent> blueTanksList = new List<TankAgent>();
    private List<TankAgent> redTanksList = new List<TankAgent>();

    [SerializeField] private int blueDeadCount = 0;
    [SerializeField] private int redDeadCount = 0;
    [SerializeField] private int blueTeamNum = 0;
    [SerializeField] private int redTeamNum = 0;

    private int randomTeamNum = 0;

    [SerializeField]
    private SoldierPool soldierPool;

    [SerializeField]
    private bool isAsymmetry;


    private SimpleMultiAgentGroup m_BlueAgentGroup;
    private SimpleMultiAgentGroup m_RedAgentGroup;

    private int m_ResetTimer;
    private float ground_length = 0;
    private float ground_weigth = 0;

    private float radius = 0;


    void Start()
    {
        // Initialize TeamManager
        m_BlueAgentGroup = new SimpleMultiAgentGroup();
        m_RedAgentGroup = new SimpleMultiAgentGroup();

        Ground ground = GetComponentInChildren<Ground>();
        ground_weigth = (int)ground.transform.localScale.x;
        ground_length = (int)ground.transform.localScale.z;
        radius = (int)ground.transform.localScale.x / 2;
        ResetScene();
    }

    void FixedUpdate()
    {
        

        if (m_ResetTimer++ >= MaxEnvironmentSteps && MaxEnvironmentSteps > 0)
        {
            Debug.Log("times up");
            m_BlueAgentGroup.GroupEpisodeInterrupted();
            m_RedAgentGroup.GroupEpisodeInterrupted();
            ResetScene();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) 
        {
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
        if (blueDeadCount == blueTeamNum)
        {
            Debug.Log("red win"+blueDeadCount+" "+ blueTeamNum);
            m_BlueAgentGroup.AddGroupReward(-(1 - (redDeadCount / redTeamNum)) * (1.5f - GameArgs.rewardRatio));
            m_RedAgentGroup.AddGroupReward((1 - (redDeadCount / redTeamNum)) * (1.5f - GameArgs.rewardRatio));
            m_BlueAgentGroup.EndGroupEpisode();
            m_RedAgentGroup.EndGroupEpisode();
            ResetScene();
            
        }
        else if (redDeadCount == redTeamNum)
        {
            Debug.Log("blue win" + redDeadCount + " " + redTeamNum);
            m_BlueAgentGroup.AddGroupReward((1 - (blueDeadCount / blueTeamNum)) * (1.5f - GameArgs.rewardRatio));
            m_RedAgentGroup.AddGroupReward(-(1 - (blueDeadCount / blueTeamNum)) * (1.5f - GameArgs.rewardRatio));
            m_BlueAgentGroup.EndGroupEpisode();
            m_RedAgentGroup.EndGroupEpisode();
            ResetScene();
        }
    }

    private void ResetScene()
    {
        GameArgs.gameCount++;
        Debug.Log(GameArgs.gameCount);
        m_ResetTimer = 0;

        //reward
        if(GameArgs.rewardRatio > 0.5f)
        {
            GameArgs.rewardRatio -= 0.00002f;
        }

        //team num
        blueDeadCount = 0;
        redDeadCount = 0;
        blueTeamNum = 0;
        redTeamNum = 0;

        randomTeamNum = Random.Range(3, 7);

        //tanklist
        blueTanksList.Clear();
        redTanksList.Clear();

        //agent list
        foreach (ClassAgent a in blueAgentsList)
        {
            m_BlueAgentGroup.UnregisterAgent(a);
            soldierPool.Rycle(Team.Blue, a.profession, a);
            if (!a.isDead) a.GameOver();
        }
        blueAgentsList.Clear();

        foreach (ClassAgent a in redAgentsList)
        {
            m_RedAgentGroup.UnregisterAgent(a);
            soldierPool.Rycle(Team.Red, a.profession, a);
            if (!a.isDead) a.GameOver();
        }
        redAgentsList.Clear();

        //env reset
        MaxEnvironmentSteps = randomTeamNum*1000;
        if (isAsymmetry)
        {
            LoadASymmetryRandomScene(Team.Blue);
            LoadASymmetryRandomScene(Team.Red);
        }
        else
        {
            LoadSymmetryRandomScene();
        }
    }

    public Vector3  findCirclePosition(Team team, List<Vector3> positions)
    {


        bool isfind = false;
        Vector3 random_position = new Vector3(0, 0, 0);
        while (!isfind)
        {
            double r = Random.Range(1, radius);
            double thetaDegrees = Random.Range(team == Team.Blue ? 1 : -1, team == Team.Blue ? 35 : -35) * 5;

            double thetaRadians = Math.PI * thetaDegrees / 180.0;
            double x = r * Math.Cos(thetaRadians);
            double z = r * Math.Sin(thetaRadians);
            random_position = new Vector3((float)x, 1.7f, (float)z);
            isfind = true;
            foreach (Vector2 position in positions)
            {
                if (Vector2.Distance(position, random_position) < 0.7f)
                {
                    isfind = false;
                    break;
                }
            }
            if (isfind) random_position += transform.position;
        }

        return random_position;
    }

    private void LoadSymmetryRandomScene()
    {
        List<Vector3> positions = new List<Vector3>();
        for (int i = 0; i < randomTeamNum; i++)
        {
            //random position
            Vector3 blueSpawnPoint = findCirclePosition(Team.Blue, positions);
            Vector3 redSpawnPoint = findCirclePosition(Team.Red, positions);

            //random soldier type
            int soldierPos = Random.Range(0, soldierTypes.Length);
            ClassAgent blueAgent = soldierPool.Spawn(Team.Blue, soldierTypes[soldierPos], blueSpawnPoint, Quaternion.Euler(0f, Random.Range(0, 360), 0f), transform);
            ClassAgent redAgent = soldierPool.Spawn(Team.Red, soldierTypes[soldierPos], redSpawnPoint, Quaternion.Euler(0f, Random.Range(0,360), 0f), transform);
            if (blueAgent is TankAgent) blueTanksList.Add((TankAgent)blueAgent);
            if (redAgent is TankAgent) redTanksList.Add((TankAgent)redAgent);
            blueAgentsList.Add(blueAgent);
            redAgentsList.Add(redAgent);
            m_BlueAgentGroup.RegisterAgent(blueAgent);
            m_RedAgentGroup.RegisterAgent(redAgent);
            blueTeamNum++;
            redTeamNum++;
        }
    }

    private void LoadASymmetryRandomScene(Team team)
    {
        SimpleMultiAgentGroup m_AgentGroup = team == Team.Blue ? m_BlueAgentGroup : m_RedAgentGroup;
        List<ClassAgent> agentList = team == Team.Blue ? blueAgentsList : redAgentsList;
        Quaternion team_rotation = Quaternion.Euler(0f, Random.Range(0, 360), 0f);

        agentList.Clear();
        List<Vector3> positions = new List<Vector3>();
        for (int i = 0; i < randomTeamNum; i++)
        {
            //random position

            Vector3 spawnPoint = findCirclePosition(team, positions);

            //random soldier type
            int soldierPos = Random.Range(0, soldierTypes.Length);
            ClassAgent agent = soldierPool.Spawn(team, soldierTypes[soldierPos], spawnPoint, team_rotation, transform);
            if (agent is TankAgent) (team == Team.Blue ? blueTanksList : redTanksList).Add((TankAgent)agent);
            agentList.Add(agent);
            m_AgentGroup.RegisterAgent(agent);
            if (team == Team.Blue)
                blueTeamNum++;
            else
                redTeamNum++;
        }
    }

    public void TurnReward(ClassAgent agent)
    {
        ClassAgent mostCloseAgent = findMostCloseEnemy(agent);

        float angle = Vector3.Angle(mostCloseAgent.transform.position - agent.transform.position, agent.transform.forward);
        float distance = Vector3.Distance(agent.transform.position, mostCloseAgent.transform.position);
        if (distance < 5)
        {
            if (angle <= 120)
            {
                agent.AddReward(0.005f * GameArgs.rewardRatio);
            }
        }
        else
        {
            if (angle <= 30)
            {
                agent.AddReward(0.005f * GameArgs.rewardRatio);
            }
            else if (angle <= 60)
            {
                agent.AddReward(0.001f * GameArgs.rewardRatio);
            }
            else if (angle >= 90)
            {
                agent.AddReward(-0.005f * GameArgs.rewardRatio);
            }
        }
        agent.AddReward(0.0002f * ((float)Math.Pow(20 - distance,2) / 15f) * GameArgs.rewardRatio);
    }

    private ClassAgent findMostCloseEnemy(ClassAgent agent)
    {
        List<ClassAgent> agentList = agent.team == Team.Blue ? redAgentsList : blueAgentsList;

        ClassAgent mostCloseAgent = agentList[0];
        float currentDistance = Vector3.Distance(agent.transform.position, mostCloseAgent.transform.position);
        for (int i = 1; i < agentList.Count; i++)
        {
            float distance = Vector3.Distance(agent.transform.position, agentList[i].transform.position);
            if (currentDistance > distance)
            {
                mostCloseAgent = agentList[i];
                currentDistance = distance;
            }
        }

        return mostCloseAgent;
    }

    //�Ŵ�
    public void DistanceReward(ClassAgent agent, float reward)
    {
        ClassAgent mostCloseAgent = findMostCloseEnemy(agent);
        float distance = Vector3.Distance(agent.transform.position, mostCloseAgent.transform.position);

        if(distance > 6)
        {
            agent.AddReward(reward);
            Debug.Log("distance2" + reward);
        }

    }
}