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
    public GameObject playerThoughts;
    TextMesh thoughts;
    DoorScript doorScript;

    bool firstTimeRead = true;
    
    // Start is called before the first frame update
    void Start()
    {
        thoughts = playerThoughts.GetComponent<TextMesh>();
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
                /*playerThoughts.SetActive(true);
                thoughts.text = "What's such a nice book doing out in an abandoned house?";*/             //text here
                ghostGirl.SetActive(true);
                doorScript = puzzleDoor.GetComponent<DoorScript>();
                doorScript.CloseDoor();
                puzzleStarted = true;
                //VN Script here
                firstTimeRead = false;
            }
            /*playerThoughts.SetActive(true);
            thoughts.text = thought2;*/

        }
    }
}
