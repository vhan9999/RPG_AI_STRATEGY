using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class EnvPlay : MonoBehaviour
{
    [SerializeField]
    private SoldierPool soldierPool;

    private List<ClassAgent> blueAgentsList = new List<ClassAgent>();
    private List<ClassAgent> redAgentsList = new List<ClassAgent>();
    public int blueCount = 0;
    public int redCount = 0;
    private int level;
    private int max_level = 5;

    private SimpleMultiAgentGroup m_BlueAgentGroup;
    private SimpleMultiAgentGroup m_RedAgentGroup;

    void Start()
    {
        Time.timeScale = 0;
        m_BlueAgentGroup = new SimpleMultiAgentGroup();
        m_RedAgentGroup = new SimpleMultiAgentGroup();
        StartLevel(0);
    }

    public void StartGame()
    {
        Time.timeScale = 1;

        foreach (ClassAgent a in blueAgentsList)
        {
            m_BlueAgentGroup.RegisterAgent(a);
        }


        foreach (ClassAgent a in redAgentsList)
        {
            m_RedAgentGroup.RegisterAgent(a);
        }
    }

    public void StartLevel(int num)
    {
        level = num;
        foreach (CharacterInfo characterInfo in LevelManager.levels[num])
        {
            Profession profession = characterInfo.Profession;
            Vector3 postion = transform.position + characterInfo.StartingPos;
            Quaternion rotation = Quaternion.Euler(0, 180f, 0);
            ClassAgent agent = soldierPool.Spawn(Team.Red, profession, postion, rotation, transform);
            redAgentsList.Add(agent);
            redCount++;
        }
    }

    public void AddCharacter(ClassAgent agent)
    {
        blueAgentsList.Add(agent);
        blueCount++;
    }

    public void RemoveCharacter(ClassAgent agent)
    {
        blueAgentsList.Remove(agent);
        soldierPool.Rycle(agent.team, agent.profession, agent);
        blueCount--;
    }

    public void DeadTouch(Team DeadTeam)
    {
        if (DeadTeam == Team.Blue)
        {
            blueCount--;
        }
        else
        {
            redCount--;
        }
        if (blueCount == 0)
        {
            EndGame(false);
        }
        else if (redCount == 0)
        {
            EndGame(true);
        }
    }
    public void EndGame(bool win)
    {
        foreach (ClassAgent a in blueAgentsList)
        {
            m_BlueAgentGroup.UnregisterAgent(a);
            soldierPool.Rycle(Team.Blue, a.profession, a);
        }
        foreach (ClassAgent a in redAgentsList)
        {
            m_RedAgentGroup.UnregisterAgent(a);
            soldierPool.Rycle(Team.Red, a.profession, a);
        }
        
        redAgentsList.Clear();
        blueAgentsList.Clear();
        blueCount = 0;
        redCount = 0;

        if(win) level = (level+1)%max_level;
        StartLevel(level);
        Time.timeScale = 0;
    }
}
