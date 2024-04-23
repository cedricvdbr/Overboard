using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    [SerializeField]
    public float _rotationSpeed = 3;

    private Quaternion _camRotation;

    // Start is called before the first frame update
    void Start()
    {
        _camRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            _camRotation.y += Input.GetAxis("Mouse X") * _rotationSpeed;

            transform.localRotation = Quaternion.Euler(_camRotation.x, _camRotation.y, _camRotation.z);
        }
    }
}
