using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{

    public float doorOpenAngle = 90f;
    private float doorOpen;
    public float openSpeed = 2f;

    public string keyItem;

    bool open = false;
    bool closing = false;
    bool enter = false;
    public bool locked = false;

    public float openTime = 1;
    private float _timer = 0;

    PlayerController playerInventory;

    Vector3 currentEulerAngles;
    Quaternion currentRotation;

    // Start is called before the first frame update
    void Start()
    {
        doorOpen =  -transform.parent.rotation.y * doorOpenAngle;
    }

    // Update is called once per frame
    void Update()
    {
        if (open == false)
        {
            if (_timer > 0)
            {
                float time = _timer / openTime;
                _timer -= Time.deltaTime;
 

                //modifying the Vector3, based on input multiplied by speed and time
                currentEulerAngles = new Vector3(0, Mathf.Lerp(doorOpen, 0, time), 0);

                //moving the value of the Vector3 into Quanternion.eulerAngle format
                currentRotation.eulerAngles =  currentEulerAngles;

                //apply the Quaternion.eulerAngles change to the gameObject
                transform.rotation = transform.parent.rotation * currentRotation;


                //transform.rotation = Quaternion.Euler(0, Mathf.Lerp(doorOpen, transform.rotation.y + 180, time), 0);

                if (_timer <= 0)
                    open = true;

            }
        }

        if (closing == true)
        {
            if (_timer > 0)
            {
                float time = _timer / openTime;
                _timer -= Time.deltaTime;


                //modifying the Vector3, based on input multiplied by speed and time
                currentEulerAngles = new Vector3(0, Mathf.Lerp(180, 0, time), 0);

                //moving the value of the Vector3 into Quanternion.eulerAngle format
                currentRotation.eulerAngles = currentEulerAngles;

                //apply the Quaternion.eulerAngles change to the gameObject
                transform.rotation = transform.parent.rotation * currentRotation;

                if (_timer <= 0)
                    open = false;
                closing = false;

            }
        }


    }

    // Activate the Main function when Player enter the trigger area
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInventory = other.GetComponent<PlayerController>();
            if (locked == true && playerInventory.inventory.Contains(keyItem))
            {
                locked = false;
                playerInventory.inventory.Remove(keyItem);
                if (_timer == 0)
                _timer = openTime;
                Debug.Log("Unlocked door and removed " + keyItem);
            }
            if (locked == true)
            {
                Debug.Log("You lack " + keyItem);
            }
            else
            {
                if (_timer == 0)
                _timer = openTime;
            }

        }
    }

    public void CloseDoor()
    {
        if (open == true)
        {
            closing = true;
            if (_timer == 0)
            _timer = openTime;
        }

    }
}
