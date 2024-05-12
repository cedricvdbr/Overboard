using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBallMovement : MonoBehaviour
{
    [SerializeField]
    private float _cannonBallSpeed = 10f;
    [SerializeField]
    private float _duration = 5f;
    [SerializeField]
    private float _playerDestroyChance = 0.25f;

    private Vector3 _initialDirection;

    void Start()
    {
        StartCoroutine(DestroyCannonBall());
    }


    void Update()
    {
        transform.Translate(_initialDirection * _cannonBallSpeed * Time.deltaTime);
        Debug.Log(transform.position.y);
    }

    public void SetInitialDirection(Vector3 direction)
    {
        _initialDirection = direction.normalized;
    }

    private IEnumerator DestroyCannonBall()
    {
        yield return new WaitForSeconds(_duration);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            float random = Random.value;
            Debug.Log(random);
            if (random < _playerDestroyChance)
            {
                Destroy(other.gameObject);
            }
            Destroy(gameObject);
        }
    }
}
