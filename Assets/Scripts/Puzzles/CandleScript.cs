using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleScript : MonoBehaviour
{

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (Input.GetButtonDown("Fire1"))
            {
                PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
                
                playerController.inventory.Add(gameObject.name);
                gameObject.SetActive(false);
            }
        }
    }
}
