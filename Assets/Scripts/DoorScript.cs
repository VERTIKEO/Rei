using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DoorScript : MonoBehaviour
{

    public float doorOpenAngle = 90f;
    private float doorOpen;
    public float openSpeed = 2f;
    public float playerSpeed = 1.0f;
    public float movementTime = 2f;
    public string keyItem;

    bool open = false;
    bool closing = false;
    bool playerClose = false;
    public bool locked = false;

    public float openTime = 1;
    private float _timer = 0;

    PlayerController playerInventory;
    GameObject player;
    GameObject turnPoint;
    Transform target;

    Vector3 currentEulerAngles;
    Quaternion currentRotation;

    // Start is called before the first frame update
    void Start()
    {
        doorOpen =  -transform.parent.rotation.y * doorOpenAngle;
        player = GameObject.Find("Player");
        turnPoint = GameObject.Find("LookPoint");
        target = turnPoint.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (locked == true && playerInventory.inventory.Contains(keyItem) && playerClose == true && Input.GetButtonDown("Fire1"))
        {
            locked = false;
            playerInventory.inventory.Remove(keyItem);
            if (_timer == 0)
            {
                _timer = openTime;
            }

            Debug.Log("Unlocked door and removed " + keyItem);
        }
        if (locked == true && playerClose == true && Input.GetButtonDown("Fire1"))
        {
            Debug.Log("You lack " + keyItem);
        }
        else if (Input.GetButtonDown("Fire1") && playerClose == true)
        {
            Debug.Log("Open Door");
            if (_timer == 0)
            {
                _timer = openTime;

            }
        }

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
                playerInventory.enabled = false;
                player.transform.LookAt(target);
                if (_timer <= 0.5 && _timer >= 0)
                player.transform.position += player.transform.forward * playerSpeed * Time.deltaTime;
                //transform.rotation = Quaternion.Euler(0, Mathf.Lerp(doorOpen, transform.rotation.y + 180, time), 0);

                if (_timer <= 0)
                {
                    open = true;
                    playerInventory.enabled = true;
                }

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
            playerClose = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerClose = false;
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
