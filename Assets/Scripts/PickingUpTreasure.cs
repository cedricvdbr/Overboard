using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickingUpTreasure : MonoBehaviour
{
    //player needs tag player, same w Treasure. Treasure has rigidbody and is kinematic.
    //Treasure collider is assigned to Player(capsule collider)
    private bool isTouchingTreasure = false;
    private GameObject Treasure;
    public Collider TreasureCollider;

    void Update()
    {
        if (isTouchingTreasure)
        {
            Treasure.transform.position = transform.position;
            
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Treasure")) 
        {
            if (!isTouchingTreasure)
            {
                isTouchingTreasure = true;
                Treasure = other.gameObject;
                other.enabled = false;
                Debug.Log("it touches");
            }
        }

    }
    private void OnTriggerExit(Collider other)
    {
     //when player hits/reaches collectionspot
    }
}
