using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassButtonManager : MonoBehaviour
{
    public string choosingClass = "";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ChooseClass(string classs)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            child.GetChild(1).gameObject.SetActive(false);
        }
        choosingClass = classs;
    }

    public string ChoosingClass
    {
        get { return choosingClass; }
    }
}
