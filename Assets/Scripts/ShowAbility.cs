using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowAbility : MonoBehaviour
{
    private GameObject _turnManagerGO;
    private TurnManager _turnManager;
    private PlayerControl _playerControl;
    [SerializeField]
    private Sprite[] _sprites;
    [SerializeField]
    private Image _image;
    void Update()
    {
        _turnManagerGO = GameObject.Find("TurnManager");
        if (_turnManagerGO != null )_turnManager = _turnManagerGO.GetComponent<TurnManager>();
        if (_turnManager != null)
        {
            _playerControl = _turnManager.currentPlayer;

            if (_playerControl != null)
            {
                string name = _playerControl.GetCurrentAbilityName();
                switch (name)
                {
                    default:
                        _image.enabled = false;
                        break;
                    case ("Caribbean Rum"):
                        _image.enabled = true;
                        _image.sprite = _sprites[0];
                        break;
                    case ("Cursed Compass"):
                        _image.enabled = true;
                        _image.sprite = _sprites[1];
                        break;
                    case ("Parrot's Warning"):
                        _image.enabled = true;
                        _image.sprite = _sprites[2];
                        break;
                }
            }
        }
    }
}
