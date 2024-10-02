using Google.Protobuf.Collections;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.MLAgents;
using Unity.MLAgents.Policies;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool isWatching = true;
    private bool isGameStart = false;
    private GameObject possessedPlayer;
    private ClassAgent possessedAgent = null;
    private Camera mainCamera;

    [SerializeField] private GameObject lines;
    [SerializeField] private GameObject PlayingUI;
    [SerializeField] private GameObject SettingUI;
    [SerializeField] private GameObject StartButton;
    [SerializeField] private GameObject MenuButton;
    [SerializeField] private GameObject PauseButton;
    [SerializeField] private GameObject StopButton;
    [SerializeField] private GameObject SummonBlock;

    [SerializeField] private ImageManager imageManager;
    [SerializeField] private Image attackIcon;
    [SerializeField] private Image skillIcon;

    [SerializeField] private Image attackCoolDown;
    [SerializeField] private Image skillCoolDown;
    private RectTransform attackCoolDownRectTransform;
    private RectTransform skillCoolDownRectTransform;

    [SerializeField] private GameObject hpBar;
    [SerializeField] LayerMask playerLayers;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
        attackCoolDownRectTransform = attackCoolDown.GetComponent<RectTransform>();
        skillCoolDownRectTransform = skillCoolDown.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameStart)
        {
            //���� possessed
            if (Input.GetMouseButtonDown(0) && isWatching)
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100, playerLayers))
                {
                    Possession(hit);
                }
            }
            //�Ѫ��� depossessed
            if (Input.GetKeyDown(KeyCode.Escape) && !isWatching)
            {
                Depossession();
            }

            //�ޯ�N�o
            if (isWatching == false)
            {
                SkillCoolDown();
            }
        }

        
    }

    public void GameStart()
    {
        isGameStart = true;
        Time.timeScale = 1;
        lines.SetActive(false);
    }
    public void GameOver()
    {
        if (!isWatching)
            Depossession();
        isGameStart = false;
        Time.timeScale = 0;
        SummonBlock.SetActive(true);
        lines.SetActive(true);
        PlayingUI.SetActive(false);
        SettingUI.SetActive(true);
        StartButton.SetActive(true);
        MenuButton.SetActive(true);
        PauseButton.SetActive(false);
        StopButton.SetActive(false);
    }
    public void GamePause()
    {
        Time.timeScale = 0;
    }

    public void Possession(RaycastHit hit)
    {
        //�]�w����
        possessedPlayer = hit.collider.gameObject;
        possessedPlayer.GetComponent<BehaviorParameters>().BehaviorType = BehaviorType.HeuristicOnly;
        possessedPlayer.GetComponentInChildren<Camera>().enabled = true;
        possessedPlayer.GetComponentInChildren<MouseLook>().enabled = true;
        possessedAgent = possessedPlayer.GetComponent<ClassAgent>();


        //ui�]�w
        PlayingUI.SetActive(true);
        hpBar.GetComponent<healthBar>().Player = possessedPlayer;
        ChangeSkillIcon();


        //������
        mainCamera = Camera.main;
        mainCamera.enabled = false;
        isWatching = false;

        //��ƹ�
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void Depossession()
    {
        PlayingUI.SetActive(false);
        possessedPlayer.GetComponent<BehaviorParameters>().BehaviorType = BehaviorType.InferenceOnly;
        mainCamera.enabled = true;
        possessedPlayer.GetComponentInChildren<Camera>().enabled = false;
        possessedPlayer.GetComponentInChildren<MouseLook>().enabled = false;
        possessedPlayer = null;
        possessedAgent = null;
        isWatching = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void CheckAgentDead()
    {
        if (possessedAgent != null && possessedAgent.isDead)
            Depossession();
    }
    private void ChangeSkillIcon()
    {
        switch (possessedAgent.profession)
        {
            case Profession.Archer:
                attackIcon.sprite = imageManager.shoot;
                skillIcon.sprite = imageManager.shoot;
                break;
            case Profession.Berserker:
                attackIcon.sprite = imageManager.cleave;
                skillIcon.sprite = imageManager.whirlwind;
                break;
            case Profession.Mage:
                attackIcon.sprite = imageManager.magicMissile;
                skillIcon.sprite = imageManager.fireBall;
                break;
            case Profession.Tank:
                attackIcon.sprite = imageManager.push;
                skillIcon.sprite = imageManager.warcry;
                break;
            case Profession.Warrior:
                attackIcon.sprite = imageManager.slash;
                skillIcon.sprite = imageManager.accelerate;
                break;
        }
    }

    private void SkillCoolDown()
    {
        switch (possessedAgent.profession)
        {
            case Profession.Archer:
                Bow bow = ((ArcherAgent)possessedAgent).bow;
                if (bow.IsReloading)
                {
                    attackCoolDownRectTransform.localScale = new Vector3(1, bow.cooldownTime / bow.reloadTime, 1);
                    skillCoolDownRectTransform.localScale = new Vector3(1, bow.cooldownTime / bow.reloadTime, 1);
                }
                else
                { 
                    attackCoolDownRectTransform.localScale = new Vector3(1, (bow.firePower - 10) / 30f, 1);
                    skillCoolDownRectTransform.localScale = new Vector3(1, (bow.firePower - 10) / 30f, 1);
                }
                break;
            case Profession.Berserker:
                Battleaxe battleaxe = ((BerserkerAgent)possessedAgent).battleaxe;
                if (battleaxe.IsWhirlwind)
                    skillCoolDownRectTransform.localScale = new Vector3(1, battleaxe.skillTime / battleaxe.skillDuration, 1);
                else
                    skillCoolDownRectTransform.localScale = new Vector3(1, battleaxe.cooldownTime / battleaxe.cooldown, 1);

                attackCoolDownRectTransform.localScale = new Vector3(1, battleaxe.attackTime / battleaxe.attackDuration, 1);
                break;
            case Profession.Mage:
                Book book = ((MageAgent)possessedAgent).book;
                if (book.IsSkill)
                    skillCoolDownRectTransform.localScale = new Vector3(1, book.skillTime / book.skillDuration, 1);
                else
                    skillCoolDownRectTransform.localScale = new Vector3(1, book.cooldownTime / book.cooldown, 1);

                attackCoolDownRectTransform.localScale = new Vector3(1, book.attackTime / book.attackDuration, 1);
                break;
            case Profession.Tank:
                Shield shield = ((TankAgent)possessedAgent).shield;
                WarCry warcry = ((TankAgent)possessedAgent).warcry;
                skillCoolDownRectTransform.localScale = new Vector3(1, warcry.cooldownTime / warcry.cooldown, 1);

                attackCoolDownRectTransform.localScale = new Vector3(1, shield.attackTime / shield.attackDuration, 1);
                break;
            case Profession.Warrior:
                Sword sword = ((WarriorAgent)possessedAgent).sword;
                Accelerate accelerate = ((WarriorAgent)possessedAgent).accelerate;
                if (accelerate.Status)
                    skillCoolDownRectTransform.localScale = new Vector3(1, accelerate.skillTime / accelerate.skillDuration, 1);
                else
                    skillCoolDownRectTransform.localScale = new Vector3(1, accelerate.cooldownTime / accelerate.cooldown, 1);

                attackCoolDownRectTransform.localScale = new Vector3(1, sword.attackTime / sword.attackDuration, 1);
                break;
        }
    }
}   
