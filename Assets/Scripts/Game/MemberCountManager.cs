using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MemberCountManager : MonoBehaviour
{
    private TextMeshProUGUI text;
    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }
    public void changeNum(int count)
    {
        text.text = "Member " + count + "/5";
    }

}
