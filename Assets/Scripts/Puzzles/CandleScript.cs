using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleScript : MonoBehaviour
{
    Book bookScript;
    public GameObject book;

    private void Start()
    {
        book = GameObject.Find("BookCube");
        bookScript = book.GetComponent<Book>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (Input.GetButtonDown("Fire1") && bookScript.puzzleStarted == true)
            {
                PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
                
                playerController.inventory.Add(gameObject.name);
                gameObject.SetActive(false);
            }
        }
    }
}
