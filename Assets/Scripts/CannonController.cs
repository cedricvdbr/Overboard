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

    private float _cannonBallYOffset = 0.5f;

    private AudioSource _cannonFireSound;

    private void Start()
    {
        _cannonFireSound = GameObject.Find("CannonFire").GetComponent<AudioSource>();
    }

    public void PlaceCannon()
    {
        _cannonPlaced = true;
        _cannonPosition = transform.position;
        _tileLayer = LayerMask.GetMask("Tile");
    }

    public void Shoot()
    {
        _cannonFireSound.Play();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _tileLayer))
        {
            if (hit.collider.CompareTag("Tile"))
            {
                Vector3 targetPosition = hit.collider.transform.position;
                Vector3 direction = targetPosition - transform.position;
                direction.y = 0f;

                Vector3 cannonBallSpawnPosition = new Vector3(transform.position.x, transform.position.y + _cannonBallYOffset, transform.position.z);
                GameObject cannonBall = Instantiate(_cannonBallPrefab, cannonBallSpawnPosition, Quaternion.identity);
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
