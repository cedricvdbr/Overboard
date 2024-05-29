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
    private bool _tutorialEnabled, _masherOn;
    [SerializeField]
    private int _index = 1;

    private bool _hasMenuOpened = false;

    private AudioSource _buttonClickSound;

    private void Start()
    {
        _buttonClickSound = GameObject.Find("ButtonClick").GetComponent<AudioSource>();
    }

    private void Update()
    {
        GameObject smasherGO = GameObject.Find("ButtonMashUI");
        Canvas smasher = null;

        if (smasherGO != null)smasher = smasherGO.GetComponent<Canvas>();

        if (smasher != null && smasher.enabled)
        {
            _masherOn = true;
        }
        else if (smasher != null && !smasher.enabled)
        {
            _masherOn= false;
        }
        if (!_hasMenuOpened && Input.GetKeyDown(KeyCode.H) && !_masherOn)
        {
            ShowHelp();
        }

        if (_tutorialEnabled)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                IncreaseIndex();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                DecreaseIndex();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseHelp();
            }

            switch (_index)
            {
                case 1:
                    _tutorialPage1.enabled = true;
                    _tutorialPage2.enabled = false;
                    _abilityPage.enabled = false;
                    _left.enabled = false;  // Disable left button on the 1st page
                    _right.enabled = true;  // Enable right button
                    break;
                case 2:
                    _tutorialPage1.enabled = false;
                    _tutorialPage2.enabled = true;
                    _abilityPage.enabled = false;
                    _left.enabled = true;   // Enable left button
                    _right.enabled = true;  // Enable right button
                    break;
                case 3:
                    _tutorialPage1.enabled = false;
                    _tutorialPage2.enabled = false;
                    _abilityPage.enabled = true;
                    _left.enabled = true;   // Enable left button
                    _right.enabled = false; // Disable right button on the 3rd page
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
        DisableButtons();
        StartCoroutine(LoadSceneAsync(1));
    }

    public void IncreaseIndex()
    {
        if (_index < 3)  // Ensure not to increase beyond the 3rd page
        {
            _buttonClickSound.Play();
            _index = Mathf.Clamp(_index + 1, 1, 3);
        }
    }

    public void DecreaseIndex()
    {
        if (_index > 1)  // Ensure not to decrease beyond the 1st page
        {
            _buttonClickSound.Play();
            _index = Mathf.Clamp(_index - 1, 1, 3);
        }
    }

    public void OpenAbility()
    {
        if (!_hasMenuOpened) _buttonClickSound.Play();
        if (!_hasMenuOpened) _index = 3;
        _hasMenuOpened = true;
        _tutorialEnabled = true;
        _close.enabled = true;
        _left.enabled = true;
        _right.enabled = false;  // Start with right button disabled since it opens on the 3rd page
    }

    public void CloseHelp()
    {
        _hasMenuOpened = false;
        _buttonClickSound.Play();
        _tutorialEnabled = false;
        _close.enabled = false;
        _left.enabled = false;
        _right.enabled = false;
        _index = 0;
    }

    public void ShowHelp()
    {
        if (!_hasMenuOpened) _buttonClickSound.Play();
        _hasMenuOpened = true;
        _tutorialEnabled = true;
        _close.enabled = true;
        _index = 1;
        _left.enabled = false;  // Disable left button initially
        _right.enabled = true;  // Enable right button initially
    }

    public void QuitGame()
    {
        _buttonClickSound.Play();
        DisableButtons();
        StartCoroutine(CloseApp());
    }

    private IEnumerator LoadSceneAsync(int sceneIndex)
    {
        yield return new WaitForSeconds(_buttonClickSound.clip.length);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIndex);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    private IEnumerator CloseApp()
    {
        yield return new WaitForSeconds(_buttonClickSound.clip.length);
        Application.Quit();
    }

    private void DisableButtons()
    {
        foreach (Button button in FindObjectsOfType<Button>())
        {
            button.interactable = false;
        }
    }
}
