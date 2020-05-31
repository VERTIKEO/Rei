using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnablePainting : MonoBehaviour
{
    public string candle = "Candle";
    public string rightBurn;
    public string wrongBurn;
    public bool isTorsaker = false;

    public GameObject door;
    public GameObject playerThoughts;
    public GameObject ghostGirl;
    public GameObject options;
    DoorScript doorScript;
    PlayerController playerController;
    

    public float damage = 50f;
    
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

            if (Input.GetButtonDown("Fire1") && playerController.inventory.Contains(candle))
            {
                options.SetActive(true);
                /*playerThoughts.SetActive(true);
                thoughts.text = "" + rightBurn;
                doorScript.locked = false;
                ghostGirl.SetActive(false);
                gameObject.SetActive(false);
            }
            if (Input.GetButtonDown("Fire1") && playerController.inventory.Contains("Candle"))
            {
                //thoughts.text = "" + wrongBurn;
                playerController.takeDamage(damage);
                gameObject.SetActive(false);*/
            }
        }
    }

}
