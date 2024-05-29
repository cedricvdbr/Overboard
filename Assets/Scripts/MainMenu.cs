using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Image _close, _tutorialPage1, _tutorialPage2, _abilityPage, _left, _right;
    private bool _tutorialEnabled;
    [SerializeField]
    private int _index = 1;

    private AudioSource _buttonClickSound;

    private void Start()
    {
        _buttonClickSound = GameObject.Find("ButtonClick").GetComponent<AudioSource>();
    }
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.H))
        {
            ShowHelp();
        }
        if (_tutorialEnabled)
        {
            switch (_index)
            {
                case 1:
                    _tutorialPage1.enabled = true;
                    _tutorialPage2.enabled = false;
                    _abilityPage.enabled = false;
                    break;
                case 2:
                    _tutorialPage1.enabled = false;
                    _tutorialPage2.enabled = true;
                    _abilityPage.enabled = false;
                    break;
                case 3:
                    _tutorialPage1.enabled = false;
                    _tutorialPage2.enabled= false;
                    _abilityPage.enabled = true;
                    break;
            }
        }
        else if (!_tutorialEnabled)
        {
            _tutorialPage1.enabled = false;
            _tutorialPage2.enabled = false;
            _abilityPage.enabled = false;
        }

    }
    public void PlayGame()
    {
        _buttonClickSound.Play();
        SceneManager.LoadScene(1);
    }
    public void IncreaseIndex()
    {
        _buttonClickSound.Play();
        _index = Mathf.Clamp(_index+1, 1, 3);
    }
    public void DecreaseIndex()
    {
        _buttonClickSound.Play();
        _index = Mathf.Clamp(_index-1, 1, 3);
    }
    public void OpenAbility()
    {
        _buttonClickSound.Play();
        _tutorialEnabled = true;
        _close.enabled = true;
        _index = 3;
        _left.enabled = true;
        _right.enabled = true;
    }
    public void CloseHelp()
    {
        _buttonClickSound.Play();
        _tutorialEnabled = false;
        _close.enabled = false;
        _left.enabled = false;
        _right.enabled = false;
    }
    public void ShowHelp()
    {
        _buttonClickSound.Play();
        _tutorialEnabled = true;
        _close.enabled = true;
        _index = 1;
        _left.enabled = true;
        _right.enabled = true;

    }
    public void QuitGame()
    {
        _buttonClickSound.Play();
        Application.Quit();
    }
}
