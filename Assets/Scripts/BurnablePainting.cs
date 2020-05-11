using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnablePainting : MonoBehaviour
{
    public string candle = "Candle";
    public bool isTorsaker = false;

    public GameObject door;
    DoorScript doorScript;
    PlayerController playerController;
    
    // Start is called before the first frame update
    void Start()
    {
        doorScript = door.GetComponent<DoorScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerController = other.gameObject.GetComponent<PlayerController>();

            if (Input.GetButtonDown("Fire1") && playerController.inventory.Contains(candle) && isTorsaker == true)
            {
                Debug.Log("You burn the right Painting!");
                doorScript.locked = false;
                gameObject.SetActive(false);
            }
            if (Input.GetButtonDown("Fire1") && playerController.inventory.Contains("Candle"))
            {
                Debug.Log("You burn the Painting");
                gameObject.SetActive(false);
            }
        }
    }

}
