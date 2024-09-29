using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class SummonBlock : MonoBehaviour
{
    [SerializeField] private MouseHoverDetection UI;
    [SerializeField] LayerMask clickableLayers;
    [SerializeField] LayerMask erasibleLayers;
    [SerializeField] private ClassButtonManager chooseClass;

    [SerializeField] private SoldierPool soldierPool;

    [SerializeField] private MemberCountManager memberCountManager;

    [SerializeField] private EnvPlay envplay;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100, clickableLayers) && !UI.IsMouseOver)
        {
            transform.position = hit.point;
            if (Input.GetMouseButtonDown(0))
            {
                if(chooseClass.ChoosingClass == "Erase")
                {
                    Collider[] colliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity, erasibleLayers);
                    foreach (Collider collider in colliders)
                    {
                        ClassAgent agent = collider.gameObject.GetComponent<ClassAgent>();
                        envplay.RemoveCharacter(agent);
                    }
                }
                else if(envplay.blueCount < 5 && chooseClass.ChoosingClass != "")
                {
                    ClassAgent agent = null;
                    switch (chooseClass.ChoosingClass)
                    {
                        case "Mage":
                            agent = soldierPool.Spawn(Team.Blue, Profession.Mage, transform.position + new Vector3(0, 0.8f, 0), transform.rotation);
                            break;
                        case "Warrior":
                            agent = soldierPool.Spawn(Team.Blue, Profession.Warrior, transform.position + new Vector3(0, 0.8f, 0), transform.rotation);
                            break;
                        case "Berserker":
                            agent = soldierPool.Spawn(Team.Blue, Profession.Berserker, transform.position + new Vector3(0, 0.8f, 0), transform.rotation);
                            break;
                        case "Archer":
                            agent = soldierPool.Spawn(Team.Blue, Profession.Archer, transform.position + new Vector3(0, 0.8f, 0), transform.rotation);
                            break;
                        case "Tank":
                            agent = soldierPool.Spawn(Team.Blue, Profession.Tank, transform.position + new Vector3(0, 0.8f, 0), transform.rotation);
                            break;
                    }
                    envplay.AddCharacter(agent);
                }
                memberCountManager.changeNum(envplay.blueCount);
            }
        }

    }
}
