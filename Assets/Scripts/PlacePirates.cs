using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacePirates : MonoBehaviour
{
    [SerializeField]
    private GameObject _pirate;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Tile"))
                {
                    Instantiate(_pirate, hit.transform.position, Quaternion.AngleAxis(90, Vector3.up));
                }
            }
        }
    }
}
