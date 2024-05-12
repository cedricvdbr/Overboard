using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryScreenManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("P1 Victory Screen Loaded");
            UnityEngine.SceneManagement.SceneManager.LoadScene(2);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("P2 Victory Screen Loaded");
            UnityEngine.SceneManagement.SceneManager.LoadScene(4);
        }
    }
}
