using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class SummonBlock : MonoBehaviour
{
    [SerializeField] private MouseHoverDetection UI;
    [SerializeField] LayerMask clickableLayers;
    [SerializeField] private ClassButtonManager chooseClass;

    [SerializeField] private SoldierPool soldierPool;
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
                
                switch (chooseClass.ChoosingClass)
                {
                    case "Mage":
                        soldierPool.Spawn(Team.Blue, Profession.Mage, transform.position + new Vector3(0, 1.1f, 0), transform.rotation);
                        break;
                    case "Warrior":
                        soldierPool.Spawn(Team.Blue, Profession.Warrior, transform.position + new Vector3(0, 1.1f, 0), transform.rotation);
                        break;
                    case "Berserker":
                        soldierPool.Spawn(Team.Blue, Profession.Berserker, transform.position + new Vector3(0, 1.1f, 0), transform.rotation);
                        break;
                    case "Archer":
                        soldierPool.Spawn(Team.Blue, Profession.Archer, transform.position + new Vector3(0, 1.1f, 0), transform.rotation);
                        break;
                    default:
                        Debug.LogError("Unknown class: " + chooseClass.ChoosingClass);
                        break;
                }
            }
        }

    }
}
