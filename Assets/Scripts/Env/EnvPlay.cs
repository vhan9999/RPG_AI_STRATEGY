using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvPlay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
    }

    public void StartGame()
    {
        Time.timeScale = 1;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }
}
