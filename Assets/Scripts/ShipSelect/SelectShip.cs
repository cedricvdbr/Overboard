using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectShip : MonoBehaviour
{

    [SerializeField]
    private PanelController _panelController;

    public static bool IsSelected = false;
    public static int CurrentSelectedShip = -1;

    private static int count = 0;
    public static int Player1Ship, Player2Ship;
    public static bool IsDone = false;

    private Image _p1;

    private AudioSource _buttonClickSound;

    void Start()
    {
        _buttonClickSound = GameObject.Find("ButtonClick").GetComponent<AudioSource>();
        _p1 = GameObject.Find("player1").GetComponent<Image>();
        Button button = this.gameObject.GetComponent<Button>();
        button.onClick.AddListener(SelectCurrentShip);
    }

    private void SelectCurrentShip()
    {
        _buttonClickSound.Play();
        CurrentSelectedShip = _panelController._currentPanelIndex;
        IsSelected = true;
    }

    void Update()
    {
        CheckStatus();
    }

    private void CheckStatus()
    {
        if (IsSelected)
        {
            Debug.Log(CurrentSelectedShip + " has been selected");
            if (count == 0)
            {
                Player1Ship = CurrentSelectedShip;
                _p1.enabled = false;
            }
            else if (count == 1)
            {
                Player2Ship = CurrentSelectedShip;
            }

            count++;
            IsSelected = false;

            if (count == 2)
            {
                IsDone = true;
                SwitchScenes();
            }
        }
    }

    private void SwitchScenes()
    {
        SceneManager.LoadScene(1);
    }

}
