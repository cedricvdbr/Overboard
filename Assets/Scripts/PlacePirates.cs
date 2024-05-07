using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacePirates : MonoBehaviour
{
    [SerializeField]
    private GameObject _piratePlayer1, _piratePlayer2, _turnManager;
    private int _pirateCounter = 0;
    private GameObject _name;
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
                    }
                    if (_pirateCounter >2&&hit.collider.gameObject.name == "player2tile")
                    {
                        _pirateCounter++;
                        _name = Instantiate(_piratePlayer2, hit.transform.position, Quaternion.AngleAxis(-90, Vector3.up));
                        _name.name = "pirate" + _pirateCounter;
                    }
              
            }
        }
        if(_pirateCounter == 6)
        {
            _name=Instantiate(_turnManager);
            _name.name = "TurnManager";
            _pirateCounter = 7;
        }
    }
}
