using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PickUpable : MonoBehaviour
{
    bool playerClose = false;
    public GameObject pickUpKey;
    PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerClose == true && Input.GetButtonDown("Fire1"))
        {
            //pickUpKey.SetActive(true);
            SceneManager.LoadScene("ui_Win");
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
