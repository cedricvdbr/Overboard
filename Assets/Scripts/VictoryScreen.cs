using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        DisableButtons();
        StartCoroutine(LoadSceneAsync(1));
    }

    public void MainMenu()
    {
        _buttonClickSound.Play();
        DisableButtons();
        StartCoroutine(LoadSceneAsync(0));
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

    private void DisableButtons()
    {
        foreach (Button button in FindObjectsOfType<Button>())
        {
            button.interactable = false;
        }
    }
}
