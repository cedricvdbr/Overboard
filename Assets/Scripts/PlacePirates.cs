using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlacePirates : MonoBehaviour
{
    [SerializeField]
    private GameObject _piratePlayer1, _piratePlayer2, _turnManager, _treasureP1, _treasureP2;
    private int _pirateCounter = 0, _treasureCounter = 0;
    private GameObject _name;
    private ArrayList _pawns = new ArrayList();
    [SerializeField]
    private Image _p1pirate, _p2pirate, _p1treasure, _p2treasure;

    private AudioSource _treasureDeploySound;

    private void Start()
    {
        _treasureDeploySound = GameObject.Find("TreasureDeploy").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
        if (Input.GetMouseButtonDown(0) && _pirateCounter <6)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)&& hit.collider.CompareTag("Tile"))
            {
                if (_pawns.Count == 0 && _pirateCounter <= 2 && hit.collider.gameObject.name == "player1tile")
                {
                    _pirateCounter++;
                    _name = Instantiate(_piratePlayer1, hit.transform.position, Quaternion.AngleAxis(90, Vector3.up));
                    _name.name = "pirate" + _pirateCounter;
                    PlayerControl controller = _name.GetComponent<PlayerControl>();
                    controller.PlayerNumber = 1;
                    _pawns.Add(controller);
                }
                else
                {
                     bool overlap = false;
                    for (int i = 0; i < _pawns.Count; i++)
                    {
                        if (hit.transform.position == ((PlayerControl)_pawns[i]).transform.position) overlap = true;
                    }
                    if (_pirateCounter <= 2 && hit.collider.gameObject.name == "player1tile" &&!overlap)
                    {
                            _pirateCounter++;
                            _name = Instantiate(_piratePlayer1, hit.transform.position, Quaternion.AngleAxis(90, Vector3.up));
                            _name.name = "pirate" + _pirateCounter;
                            PlayerControl controller = _name.GetComponent<PlayerControl>();
                            controller.PlayerNumber = 1;
                            _pawns.Add(controller);
                    }
                    if (_pirateCounter > 2 && hit.collider.gameObject.name == "player2tile"&& !overlap)
                    {
                            _pirateCounter++;
                            _name = Instantiate(_piratePlayer2, hit.transform.position, Quaternion.AngleAxis(-90, Vector3.up));
                            _name.name = "pirate" + _pirateCounter;
                            PlayerControl controller = _name.GetComponent<PlayerControl>();
                            controller.PlayerNumber = 2;
                            _pawns.Add(controller);
                    }
                    
                }
            }
        }
        if(Input.GetMouseButtonDown(0) && _treasureCounter <6 && _pirateCounter==6)
        {
            Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit) && hit.collider.CompareTag("Tile"))
            {
                if (hit.collider.gameObject.name == "player1tile" && _treasureCounter <= 2)
                {
                    _treasureCounter++;
                    _name = Instantiate(_treasureP1, hit.transform.position, Quaternion.AngleAxis(90, Vector3.up));
                    _treasureDeploySound.Play();
                    _name.name = "player1treasure";
                }
                if (hit.collider.gameObject.name == "player2tile" && _treasureCounter > 2)
                {
                    _treasureCounter++;
                    _name = Instantiate(_treasureP2, hit.transform.position, Quaternion.AngleAxis(-90, Vector3.up));
                    _treasureDeploySound.Play();
                    _name.name = "player2treasure";
                }
            }
        }
        if(_pirateCounter == 6 && _treasureCounter == 6)
        {
            foreach (PlayerControl controller in _pawns)
            {
                controller.GetPlayers();
            }
            _name=Instantiate(_turnManager);
            _name.name = "TurnManager";
            _pirateCounter = 7;
            _treasureCounter = 7;
        }
    }

    private void UpdateUI()
    {
        if(_pirateCounter <= 2)
        {
            _p1pirate.enabled = true;
            _p2pirate.enabled = false;
            _p1treasure.enabled = false;
            _p2treasure.enabled = false;
        }
        if (_pirateCounter > 2)
        {
            _p1pirate.enabled = false;
            _p2pirate.enabled = true;
            _p1treasure.enabled = false;
            _p2treasure.enabled = false;
        }
        if (_treasureCounter <= 2 && _pirateCounter == 6)
        {
            _p1pirate.enabled = false;
            _p2pirate.enabled = false;
            _p1treasure.enabled = true;
            _p2treasure.enabled = false;
        }
        if (_treasureCounter > 2 && _pirateCounter == 6)
        {
            _p1pirate.enabled = false;
            _p2pirate.enabled = false;
            _p1treasure.enabled = false;
            _p2treasure.enabled = true;
        }
        if( _treasureCounter >6 && _pirateCounter > 6)
        {
            _p1pirate.enabled = false;
            _p2pirate.enabled = false;
            _p1treasure.enabled = false;
            _p2treasure.enabled = false;
        }

    }
}
