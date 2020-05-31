using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleScript : MonoBehaviour
{
    bool playerClose;
    Book bookScript;
    public GameObject book;
    public PlayerController playerController;
    public GameObject candlePickup;

    private void Start()
    {
        book = GameObject.Find("BookCube");
        bookScript = book.GetComponent<Book>();
        GameObject player = GameObject.Find("Player");
        playerController = player.gameObject.GetComponent<PlayerController>();
        candlePickup = GameObject.Find("CandleFlowchart");
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && bookScript.puzzleStarted == true && playerClose == true)
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
