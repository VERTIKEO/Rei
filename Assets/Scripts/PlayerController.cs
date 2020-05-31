using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    CharacterController characterController;
    public GameObject gameOver;
    Animator animator;

    [Header("Animations")]
    public float animationSpeed = 0.5f;

    [Header("Movement")]
    [SerializeField] private Vector3 moveDirection = Vector3.zero;
    public float speed = 5.0f;
    public float gravity = 10;

    [Header("Health")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Inventory")]
    public List<string> inventory;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        currentHealth = maxHealth;
        gameOver = GameObject.Find("GameOverFlowchart");
    }

    // Update is called once per frame
    void Update()
    {
        bool movementButtons = Input.GetAxisRaw("Horizontal") != 0f || Input.GetAxisRaw("Vertical") != 0f;

        if (characterController.isGrounded)
        {
            moveDirection = movementButtons ? new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical")).normalized : Vector3.zero;
        }

        if (movementButtons)
        {
            float angle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            angle += Camera.main.transform.rotation.eulerAngles.y;  // calculate angle relative to camera
            
            transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
        }

        // SimpleMove automatically applies gravity
        Vector3 forward = movementButtons ? transform.TransformDirection(Vector3.forward) : Vector3.zero;
        characterController.SimpleMove(forward * speed);
      
        // Apply animations
        if(movementButtons) {
            // set walking animation
            animator.SetBool("isWalking", true);
            animator.speed = animationSpeed * speed;
        } else {
            // set idle animation
            animator.SetBool("isWalking", false);
            animator.speed = 1;
        }

        if (currentHealth <= 0f)
        {
            SceneManager.LoadScene(2);
        }
    }

    public void SetEnable()
    {
        this.enabled = true;

        Camera.main.GetComponent<Cinemachine.CinemachineBrain>().enabled = true;
    }

    public void SetDisable()
    {
        this.enabled = false;

        Cinemachine.CinemachineBrain brainCamera = Camera.main.GetComponent<Cinemachine.CinemachineBrain>();
        //StartCoroutine("RecenterAndDisableCamera", brainCamera);
        brainCamera.enabled = false;
    }

    private IEnumerable CameraRecenterAndDisable(Cinemachine.CinemachineBrain brainCamera) {
        Cinemachine.CinemachineFreeLook camera = brainCamera.ActiveVirtualCamera as Cinemachine.CinemachineFreeLook;
        
        //camera.m_Heading.m_Bias = 120;
        camera.m_RecenterToTargetHeading.RecenterNow();
        yield return new WaitForSeconds(camera.m_RecenterToTargetHeading.m_RecenteringTime + 0.1f);
        brainCamera.enabled = false;    // ...or anything else to reenable inputs
    }

    public void takeDamage(float takenDamage)
    {
        currentHealth -= takenDamage;
    }
}
