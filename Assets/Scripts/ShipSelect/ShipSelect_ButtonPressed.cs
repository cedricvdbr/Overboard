using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShipSelect_ButtonPressed : MonoBehaviour
{
    [SerializeField]
    private PanelController _panelController;

    private AudioSource _buttonClickSound;

    // Start is called before the first frame update
    void Start()
    {
        _buttonClickSound = GameObject.Find("ButtonClick").GetComponent<AudioSource>();
        Button button = this.gameObject.GetComponent<Button>();
        button.onClick.AddListener(ChangePanel);
    }

    private void ChangePanel()
    {
        _buttonClickSound.Play();
        Debug.Log("Button '" + this.name + "' has been pressed");
        TextMeshProUGUI tmp = this.GetComponentInChildren<TextMeshProUGUI>();
        int indexChange = int.Parse(tmp.text);
        _panelController.SetCurrentPanel(indexChange);
    }

    
}
