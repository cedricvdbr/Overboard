using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlacePirates : MonoBehaviour
{
    [SerializeField]
    private GameObject _piratePlayer1, _piratePlayer2, _turnManager, _treasureP1, _treasureP2;
    private int _pirateCounter = 0, _treasureCounter = 0;
    private GameObject _name;
    private ArrayList _pawns = new ArrayList();
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(2) && _pirateCounter <6)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)&& hit.collider.CompareTag("Tile"))
            {
                if (_pirateCounter <=2&&hit.collider.gameObject.name == "player1tile")
                {
                    _pirateCounter++;
                    _name = Instantiate(_piratePlayer1, hit.transform.position, Quaternion.AngleAxis(90, Vector3.up));
                    _name.name = "pirate"+ _pirateCounter;
                    PlayerControl controller = _name.GetComponent<PlayerControl>();
                    controller.PlayerNumber = 1;
                    _pawns.Add(controller);
                }
                if (_pirateCounter >2&&hit.collider.gameObject.name == "player2tile")
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
        if(Input.GetMouseButtonDown(2) && _treasureCounter <6 && _pirateCounter==6)
        {
            Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit) && hit.collider.CompareTag("Tile"))
            {
                if (hit.collider.gameObject.name == "player1tile" && _treasureCounter <= 2)
                {
                    _treasureCounter++;
                    _name = Instantiate(_treasureP1, hit.transform.position, Quaternion.AngleAxis(90, Vector3.up));
                    _name.name = "player1treasure";
                }
                if (hit.collider.gameObject.name == "player2tile" && _treasureCounter > 2)
                {
                    _treasureCounter++;
                    _name = Instantiate(_treasureP2, hit.transform.position, Quaternion.AngleAxis(-90, Vector3.up));
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
}
