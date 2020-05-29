using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleScript : MonoBehaviour
{
    Book bookScript;
    GameObject intro;
    public GameObject book;
    public PlayerController playerController;

    private void Start()
    {
        book = GameObject.Find("BookCube");
        bookScript = book.GetComponent<Book>();
        GameObject player = GameObject.Find("Player");
        playerController = player.gameObject.GetComponent<PlayerController>();
        intro = GameObject.Find("Intro");
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (Input.GetButtonDown("Fire1") && bookScript.puzzleStarted == true)
            {
                intro.SendMessage("CandlePickup");
                

            }
        }
    }

    public void PickUp()
    {
        playerController.inventory.Add(gameObject.name);
        gameObject.SetActive(false);
    }
}
