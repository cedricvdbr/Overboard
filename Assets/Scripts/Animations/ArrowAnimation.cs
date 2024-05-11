using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowAnimation : MonoBehaviour
{
    [SerializeField]
    private float _speed, _rotationSpeed;

    // Update is called once per frame
    void Update()
    {
        float y = Mathf.PingPong(Time.time * _speed, 1) +2;
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
        transform.Rotate(Vector3.up, _rotationSpeed);
    }
}
