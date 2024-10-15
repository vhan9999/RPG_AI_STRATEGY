using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.MLAgents;
using UnityEngine;

public class EnvPlay : MonoBehaviour, IEnvController
{
    [SerializeField]
    private SoldierPool soldierPool;

    private List<ClassAgent> blueAgentsList = new List<ClassAgent>();
    private List<ClassAgent> redAgentsList = new List<ClassAgent>();
    
    public GameManager gameManager;
    public TextMeshProUGUI winLoseText;
    public TextMeshProUGUI levelText;
    public int blueCount = 0;
    public int redCount = 0;
    public RealObjectPool objectPool;
    private int level;
    private int max_level = 5;
    private bool isWin = false;

    private SimpleMultiAgentGroup m_BlueAgentGroup;
    private SimpleMultiAgentGroup m_RedAgentGroup;

    void Start()
    {
        Time.timeScale = 0;
        m_BlueAgentGroup = new SimpleMultiAgentGroup();
        m_RedAgentGroup = new SimpleMultiAgentGroup();
    }

    public void StartLevel(int num)
    {
        level = num;
        levelText.text = "Level " + (level+1);  
        foreach (CharacterInfo characterInfo in LevelManager.levels[num])
        {
            Profession profession = characterInfo.Profession;
            Vector3 postion = transform.position + characterInfo.StartingPos;
            Quaternion rotation = Quaternion.Euler(0, 180f, 0);
            ClassAgent agent = soldierPool.Spawn(Team.Red, profession, postion, rotation, transform);
            redAgentsList.Add(agent);
            m_RedAgentGroup.RegisterAgent(agent);
            redCount++;
        }
    }

    public void AddCharacter(ClassAgent agent)
    {
        blueAgentsList.Add(agent);
        m_BlueAgentGroup.RegisterAgent(agent);
        blueCount++;
    }

    public void RemoveCharacter(ClassAgent agent)
    {
        blueAgentsList.Remove(agent);
        soldierPool.Rycle(agent.team, agent.profession, agent);
        m_BlueAgentGroup.UnregisterAgent(agent);
        blueCount--;
    }

    public void DeadTouch(Team DeadTeam)
    {
        if (DeadTeam == Team.Blue)
        {
            blueCount--;
            gameManager.CheckAgentDead();
        }
        else
        {
            redCount--;
        }
        if (blueCount == 0)
        {
            ShowGameEnd(false);
        }
        else if (redCount == 0)
        {
            ShowGameEnd(true);
        }
    }
    public void ShowGameEnd(bool win)
    {
        winLoseText.gameObject.SetActive(true);
        winLoseText.text = win ? "You Won" : "You Lose";

        isWin = win;
        Invoke("EndGame", 2f);
    }
    public void EndGame()
    {
        winLoseText.gameObject.SetActive(false);

        ClearScene();



        if (isWin) level = (level+1)%max_level;
        StartLevel(level);
        isWin = false;
    }
    public void ClearScene()
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

        objectPool.RecycleAll();

        redAgentsList.Clear();
        blueAgentsList.Clear();
        blueCount = 0;
        redCount = 0;

        gameManager.GameOver();
    }
}
