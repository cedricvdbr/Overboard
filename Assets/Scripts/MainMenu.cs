using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Image _tutorial, _close;
    private void Start()
    {
    }
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.H))
        {
            ShowHelp();
        }

    }
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }
    public void CloseHelp()
    {
        _tutorial.enabled = false;
        _close.enabled = false;
    }
    public void ShowHelp()
    {
        _tutorial.enabled = true;
        _close.enabled = true;

    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
