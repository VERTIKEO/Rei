using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController characterController;
    public float speed = 5.0f;
    private Vector3 moveDirection = Vector3.zero;
    public float gravity = 10;

    public float maxHealth = 100f;
    public float currentHealth;

    public List<string> inventory;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //moveDirection = Vector3.zero;   // reset movement vector
        if (characterController.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical")).normalized;
        }

        if (Input.GetAxisRaw("Horizontal") != 0f || Input.GetAxisRaw("Vertical") != 0f)
        {
            float angle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            angle += Camera.main.transform.rotation.eulerAngles.y;  // calculate angle relative to camera
            
            transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
        }

        // SimpleMove automatically applies gravity
        Vector3 forward = movementButtons ? transform.TransformDirection(Vector3.forward) : Vector3.zero;
        characterController.SimpleMove(forward * speed);
      
        // Apply animations

        /*if (currentHealth <= 0f)
        {
            GameOver();
        }*/
    }

    public void takeDamage(float takenDamage)
    {
        currentHealth -= takenDamage;
    }
}
