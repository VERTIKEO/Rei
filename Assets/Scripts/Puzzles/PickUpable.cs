using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUpable : MonoBehaviour
{

    GameObject textBox;
    // Start is called before the first frame update
    void Start()
    {
        textBox = GameObject.Find("Intro");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (Input.GetButtonDown("Fire1"))
            {
                
                PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
                playerController.inventory.Add(gameObject.name);
                textBox.SendMessage("PickedUpKey");
                
                gameObject.SetActive(false);
            }
        }
    }
}
