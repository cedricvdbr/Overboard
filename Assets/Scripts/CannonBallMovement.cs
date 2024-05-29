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
    public static float _playerDestroyChance = 0.25f;

    private Vector3 _initialDirection;

    private AudioSource _playerHitSound;
    private AudioSource _cannonBallDespawnSound;

    void Start()
    {
        _playerHitSound = GameObject.Find("PlayerHit").GetComponent<AudioSource>();
        _cannonBallDespawnSound = GameObject.Find("CannonBallDespawn").GetComponent<AudioSource>();
        StartCoroutine(DestroyCannonBall());
    }


    void Update()
    {
        transform.Translate(_initialDirection * _cannonBallSpeed * Time.deltaTime);
        //Debug.Log(transform.position.y);
        Debug.Log("destroychance " + _playerDestroyChance);
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
        if (other.CompareTag("Wall"))
        {
            _cannonBallDespawnSound.Play();
            Destroy(gameObject);
        }

        if (other.CompareTag("Player"))
        {
            float random = Random.value;
            Debug.Log(random);
            if (random < _playerDestroyChance)
            {
                PlayerControl hit = other.gameObject.GetComponent<PlayerControl>();
                hit.IsKO = true;
                hit.PlaceX();
                _playerHitSound.Play();
                Destroy(gameObject);
            }
            else
            {
                _cannonBallDespawnSound.Play();
                Destroy(gameObject);
            }
        }
    }
}
