using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowAbility : MonoBehaviour
{
    private TurnManager _turnManager;
    private PlayerControl _playerControl;

    void Update()
    {
        
        _turnManager = GameObject.Find("TurnManager").GetComponent<TurnManager>();
        if (_turnManager != null)
        {
            _playerControl = _turnManager.currentPlayer;
            if (_playerControl != null)
            {
                TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
                text.text = _playerControl.GetCurrentAbilityName();
            }
        }
    }
}
