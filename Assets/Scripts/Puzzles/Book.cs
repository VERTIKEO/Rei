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
    
    // Start is called before the first frame update
    void Start()
    {
        bookText = GameObject.Find("BookFlowchart");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        
        
        if (other.gameObject.tag == "Player" && Input.GetButtonDown("Fire1"))
        {

            if (firstTimeRead == true)
            {
                //doorScript = puzzleDoor.GetComponent<DoorScript>();
                //doorScript.CloseDoor();
                bookText.SendMessage("FirstRead");
                puzzleStarted = true;

                firstTimeRead = false;
            }
            else
            bookText.SendMessage("Read");

        }
    }
}
