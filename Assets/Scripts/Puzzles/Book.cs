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
        if (playerClose == true && puzzleStarted == false && Input.GetButtonDown("Fire1"))
        {
            puzzleStarted = true;

            Fungus.Vector3Variable ghostPos = new Fungus.Vector3Variable();
            ghostPos.Value = ghostGirl.transform.position;
            bookText.GetComponent<Fungus.Flowchart>().SetVariable<Fungus.Vector3Variable>("ghostPos", ghostPos);
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
