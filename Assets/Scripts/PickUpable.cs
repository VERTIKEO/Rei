﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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

                Debug.Log("picked up " + gameObject);
                gameObject.SetActive(false);
            }
        }
    }
}
