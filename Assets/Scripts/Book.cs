using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour
{
    public GameObject ghostGirl;
    public GameObject puzzleDoor;
    DoorScript doorScript;

    bool firstTimeRead = true;
    
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
        
        
        if (other.gameObject.tag == "Player" && Input.GetButtonDown("Fire1"))
        {
            //VN Script Here
            if (firstTimeRead = true)
            {
                ghostGirl.SetActive(true);
                doorScript = puzzleDoor.GetComponent<DoorScript>();
                doorScript.CloseDoor();
                //VN Script here
                firstTimeRead = false;
            }

        }
    }
}
