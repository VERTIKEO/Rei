using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleScript : MonoBehaviour
{
    bool playerClose = false;
    public Book book;
    public PlayerController playerController;
    public GameObject candlePickup;

    private void Start()
    {
        playerController = GameObject.FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        if (playerClose == true && book.puzzleStarted == true && Input.GetButtonDown("Fire1"))
        {
            candlePickup.SetActive(true);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerClose = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerClose = false;
        }
    }

    public void PickUp()
    {
        playerController.inventory.Add(gameObject.name);
        gameObject.SetActive(false);
    }
}
