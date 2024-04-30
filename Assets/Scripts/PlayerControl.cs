using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerControl : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;

    private Vector3 _targetPosition;

    private bool _isMoving;

    // Start is called before the first frame update
    void Start()
    {
        _targetPosition = transform.position;
        _isMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        MovePlayer();
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Tile"))
                {
                    if (!_isMoving)
                    {
                        _targetPosition = new Vector3(hit.transform.position.x, transform.position.y, hit.transform.position.z);
                        _isMoving = true;
                    }
                }
            }
        }
    }

    private void MovePlayer()
    {
        if (_isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _speed * Time.deltaTime);

            if (transform.position == _targetPosition)
            {
                _isMoving = false;
            }
        }
    }
}
