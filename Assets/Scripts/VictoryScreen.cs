using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScreen : MonoBehaviour
{
    private AudioSource _buttonClickSound;

    private void Start()
    {
        _buttonClickSound = GameObject.Find("ButtonClick").GetComponent<AudioSource>();
    }
    public void PlayAgain()
    {
        _buttonClickSound.Play();
        SceneManager.LoadScene(1);
    }

    public void MainMenu()
    {
        _buttonClickSound.Play();
        SceneManager.LoadScene(0);
    }
}
