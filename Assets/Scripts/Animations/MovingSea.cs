using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MovingSea : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    void Update()
    {
        float y = Mathf.PingPong(Time.time * _speed, 1) * -1 -1;
        transform.position = new Vector3(0, y, 0);
    }
}
