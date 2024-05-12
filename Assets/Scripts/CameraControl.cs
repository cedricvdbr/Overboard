using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    [SerializeField]
    public float _rotationSpeed = 3;
    [SerializeField]
    private Quaternion _camRotation;

    private int _counter = 0;

    // Start is called before the first frame update
    void Start()
    {
        _camRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.mouseScrollDelta != Vector2.zero)
        {
            Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView -= Input.mouseScrollDelta.y, 40, 60);
        }
        if (Input.GetMouseButton(1))
        {
            _counter++;
            _camRotation.y += Input.GetAxis("Mouse X") * _rotationSpeed;
            _camRotation.z += Input.GetAxis("Mouse Y") * _rotationSpeed*(-1);
            if (_camRotation.z > 30) _camRotation.z = 30;
            if (_camRotation.z < -30) _camRotation.z = -30;
            if (_counter == 1 && _camRotation.y < 150) _camRotation.y = 90;
            transform.localRotation = Quaternion.Euler(_camRotation.x, _camRotation.y, _camRotation.z);
        }
    }
}
