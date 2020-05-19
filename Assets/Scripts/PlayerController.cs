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
    public float takenDamage;

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
            transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
        }

        moveDirection.y -= gravity * Time.deltaTime;
      
        characterController.Move(moveDirection * speed * Time.deltaTime);

        /*if (currentHealth <= 0f)
        {
            GameOver();
        }*/
    }

    public void takeDamage()
    {
        currentHealth -= takenDamage;
    }
}
