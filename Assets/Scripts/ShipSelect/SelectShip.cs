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

    void Start()
    {
        Button button = this.gameObject.GetComponent<Button>();
        button.onClick.AddListener(SelectCurrentShip);
    }

    private void SelectCurrentShip()
    {
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
        SceneManager.LoadScene(0);
    }

}
