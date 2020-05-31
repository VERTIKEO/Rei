using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour
{
    public string thought;
    public string thought2;
    public bool puzzleStarted = false;
    public GameObject ghostGirl;
    public GameObject puzzleDoor;
    DoorScript doorScript;
    public GameObject bookText;
    bool firstTimeRead = true;
    bool playerClose = false;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && playerClose == true)
        {
            if (puzzleStarted == false)
            {
                puzzleStarted = true;
            }
            bookText.SetActive(true);
  
        }
 
    }

    private void OnTriggerEnter(Collider other)
    {
               
        if (other.gameObject.name == "Player")
        {
            playerClose = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            playerClose = false;
        }
    }
}
