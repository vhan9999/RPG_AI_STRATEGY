using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;
using System.Linq;

using Random = UnityEngine.Random;
using UnityEngine.UIElements;

public class EnvControlleraaa : MonoBehaviour
{
    //Max Academy steps before this platform resets
    [Tooltip("Max Environment Steps")] public int MaxEnvironmentSteps = 14000;

    private Profession[] soldierTypes = { Profession.Warrior, Profession.Berserker, Profession.Mage, Profession.Archer, Profession.Tank };

    //List of Agents On Platform
    private List<ClassAgent> blueAgentsList = new List<ClassAgent>();
    private List<ClassAgent> redAgentsList = new List<ClassAgent>();
    private List<TankAgent> blueTanksList = new List<TankAgent>();
    private List<TankAgent> redTanksList = new List<TankAgent>();

    [SerializeField] private int blueDeadCount = 0;
    [SerializeField] private int redDeadCount = 0;
    [SerializeField] private int blueteamNum = 0;
    [SerializeField] private int redTeamNum = 0;

    [SerializeField]
    private SoldierPool soldierPool;

    [SerializeField]
    private bool isAsymmetry;


    private SimpleMultiAgentGroup m_BlueAgentGroup;
    private SimpleMultiAgentGroup m_RedAgentGroup;

    private int m_ResetTimer;
    private float ground_length = 0;
    private float ground_weigth = 0;


    void Start()
    {
        // Initialize TeamManager
        m_BlueAgentGroup = new SimpleMultiAgentGroup();
        m_RedAgentGroup = new SimpleMultiAgentGroup();

        Ground ground = GetComponentInChildren<Ground>();
        ground_weigth = (int)ground.transform.localScale.x;
        ground_length = (int)ground.transform.localScale.z;
        Debug.Log(ground_weigth);
        Debug.Log(ground_length);
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
        if (Input.GetKeyDown(KeyCode.S)) 
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

    private void LoadSymmetryRandomScene()
    {

        foreach (ClassAgent a in blueAgentsList)
        {
            m_BlueAgentGroup.UnregisterAgent(a);
            soldierPool.Rycle(Team.Blue, a.profession, a);
        }
        blueAgentsList.Clear();

        foreach (ClassAgent a in redAgentsList)
        {
            m_RedAgentGroup.UnregisterAgent(a);
            soldierPool.Rycle(Team.Red, a.profession, a);
        }
        redAgentsList.Clear();

        List<Vector3> positions = new List<Vector3>();
        for (int i = 0; i < 5; i++)
        {
            //random position
            Vector3 blueSpawnPoint = findPostion(Team.Blue, positions);
            Vector3 redSpawnPoint = findPostion(Team.Red, positions);

            //random soldier type
            int soldierPos = Random.Range(0, soldierTypes.Length);
            ClassAgent blueAgent = soldierPool.Spawn(Team.Blue, soldierTypes[soldierPos], blueSpawnPoint, Quaternion.Euler(0f, 0f, 0f), transform);
            ClassAgent redAgent = soldierPool.Spawn(Team.Red, soldierTypes[soldierPos], redSpawnPoint, Quaternion.Euler(0f, 180f, 0f), transform);
            if (blueAgent is TankAgent) blueTanksList.Add((TankAgent)blueAgent);
            if (redAgent is TankAgent) redTanksList.Add((TankAgent)redAgent);
            blueAgentsList.Add(blueAgent);
            redAgentsList.Add(redAgent);
            m_BlueAgentGroup.RegisterAgent(blueAgent);
            m_RedAgentGroup.RegisterAgent(redAgent);
            blueteamNum++;
            redTeamNum++;
        }
    }

    private void LoadASymmetryRandomScene(Team team)
    {
        SimpleMultiAgentGroup m_AgentGroup = team == Team.Blue ? m_BlueAgentGroup : m_RedAgentGroup;
        List<ClassAgent> agentList = team == Team.Blue ? blueAgentsList : redAgentsList;
        Quaternion team_rotation = Quaternion.Euler(0f, team == Team.Blue ? 0f : 180f, 0f);

        foreach (ClassAgent a in agentList) 
        {
            m_AgentGroup.UnregisterAgent(a);
            soldierPool.Rycle(team, a.profession, a);
        }
        agentList.Clear();
        List<Vector3> positions = new List<Vector3>();
        for (int i = 0; i < 5; i++)
        {
            //random position

            Vector3 spawnPoint = findPostion(team, positions);

            //random soldier type
            int soldierPos = Random.Range(0, soldierTypes.Length);
            ClassAgent agent = soldierPool.Spawn(team, soldierTypes[soldierPos], spawnPoint, team_rotation, transform);
            if (agent is TankAgent) (team == Team.Blue ? blueTanksList : redTanksList).Add((TankAgent)agent);
            agentList.Add(agent);
            m_AgentGroup.RegisterAgent(agent);
            if (team == Team.Blue)
                blueteamNum++;
            else
                redTeamNum++;
        }
    }

    Vector3 findPostion(Team team, List<Vector3> positions)
    {
        float team_length_pos_init = team == Team.Blue ? -0.45f * ground_length : 0.45f * ground_length;
        float team_length_pos_end = team == Team.Blue ? -0.05f * ground_length : 0.05f * ground_length;

        bool isfind = false;
        Vector3 random_position = transform.position;
        while (!isfind)
        {
            Vector3 random_postion = new Vector3(Random.Range(-ground_weigth / 2, ground_weigth / 2), 1.7f, Random.Range(team_length_pos_init, team_length_pos_end));
            isfind = true;
            foreach (Vector2 position in positions)
            {
                if (Vector2.Distance(position, random_postion) < 0.7f)
                {
                    isfind = false;
                    break;
                }
            }
            if (isfind) random_position += random_postion;
        }

        return random_position;
    }
    public void tankPenalty(Team team, float teammatePenalty)
    {
        List<TankAgent> tankList = team == Team.Blue ? blueTanksList : redTanksList;
        foreach (TankAgent tankAgent in tankList)
            if (GameArgs.IsDense) tankAgent?.AddReward(teammatePenalty / (5-tankList.Count));
    }
}