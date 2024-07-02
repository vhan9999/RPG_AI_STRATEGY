using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class SummonBlock : MonoBehaviour
{
    [SerializeField] private MouseHoverDetection UI;
    [SerializeField] LayerMask clickableLayers;
    [SerializeField] private ClassButtonManager chooseClass;
    [SerializeField] private GameObject magePrefab;
    [SerializeField] private GameObject warriorPrefab;
    [SerializeField] private GameObject berserkerPrefab;
    [SerializeField] private GameObject archerPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, clickableLayers) && !UI.IsMouseOver)
        {
            transform.position = hit.point;
            if (Input.GetMouseButtonDown(0))
            {
                switch (chooseClass.ChoosingClass)
                {
                    case "Mage":
                        Instantiate(magePrefab, transform.position + new Vector3(0, 1.1f, 0), transform.rotation);
                        break;
                    case "Warrior":
                        Instantiate(warriorPrefab, transform.position + new Vector3(0, 1.1f, 0), transform.rotation);
                        break;
                    case "Berserker":
                        Instantiate(berserkerPrefab, transform.position + new Vector3(0, 1.1f, 0), transform.rotation);
                        break;
                    case "Archer":
                        Instantiate(archerPrefab, transform.position + new Vector3(0, 1.1f, 0), transform.rotation);
                        break;
                    default:
                        Debug.LogError("Unknown class: " + chooseClass.ChoosingClass);
                        break;
                }
            }
        }

    }
}
