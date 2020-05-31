using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUpable : MonoBehaviour
{
    bool playerClose = false;
    GameObject pickUpKey;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        pickUpKey = GameObject.Find("KeyFlowchart");
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && playerClose == true)
        {
            pickUpKey.SetActive(true);

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
        PlayerController playerController = player.gameObject.GetComponent<PlayerController>();
        playerController.inventory.Add(gameObject.name);

        gameObject.SetActive(false);
    }
}
