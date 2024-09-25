using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class EnvPlay : MonoBehaviour
{
    [SerializeField]
    private SoldierPool soldierPool;
    public int level = 0;

    private List<ClassAgent> blueAgentsList = new List<ClassAgent>();
    private List<ClassAgent> redAgentsList = new List<ClassAgent>();
    private int blueCount = 0;
    private int redCount = 0;

    private SimpleMultiAgentGroup m_BlueAgentGroup;
    private SimpleMultiAgentGroup m_RedAgentGroup;

    void Start()
    {
        Time.timeScale = 0;
        StartLevel();
    }

    public void StartGame()
    {
        Time.timeScale = 1;

        foreach (ClassAgent a in blueAgentsList)
        {
            m_BlueAgentGroup.UnregisterAgent(a);
        }

        foreach (ClassAgent a in redAgentsList)
        {
            m_RedAgentGroup.UnregisterAgent(a);
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void StartLevel()
    {
        foreach (CharacterInfo characterInfo in LevelManager.levels[level])
        {
            Profession profession = characterInfo.Profession;
            Vector3 postion = transform.position + characterInfo.StartingPos;
            Quaternion rotation = Quaternion.Euler(0, 180f, 0);
            Debug.Log(postion);
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

        if(win) level++;
        StartLevel();
        Time.timeScale = 0;
    }
}
