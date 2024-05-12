using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    [SerializeField]
    private GameObject _cannonBallPrefab;

    private bool _cannonPlaced = false;
    private Vector3 _cannonPosition;

    private LayerMask _tileLayer;

    public void PlaceCannon()
    {
        _cannonPlaced = true;
        _cannonPosition = transform.position;
        _tileLayer = LayerMask.GetMask("Tile");
    }

    public void Shoot()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _tileLayer))
        {
            if (hit.collider.CompareTag("Tile"))
            {
                Vector3 targetPosition = hit.collider.transform.position;
                Vector3 direction = targetPosition - transform.position;
                direction.y = 0f;

                GameObject cannonBall = Instantiate(_cannonBallPrefab, transform.position, Quaternion.identity);
                CannonBallMovement ballMovement = cannonBall.GetComponent<CannonBallMovement>();

                if (ballMovement != null)
                {
                    ballMovement.SetInitialDirection(direction);
                }
                else
                {
                    Debug.LogError("CannonBallMovement component not found!");
                }
            }
            else
            {
                Debug.Log("Clicked on something other than a tile!");
            }
        }
    }

    public bool IsCannonPlaced()
    {
        return _cannonPlaced;
    }

    public Vector3 GetCannonPosition()
    {
        return _cannonPosition;
    }
}