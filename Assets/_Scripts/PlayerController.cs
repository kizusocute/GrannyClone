using System.Collections;
using System.Collections.Generic;
using Terresquall;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float currentHealth;
    float maxHeatlh = 100f;

    public float speed = 3f;
    private Vector3 velocity;
    private CharacterController characterController;
    bool isGrounded;
    const float gravity = -9.81f; // Gravity constant

    public float mouseSensitivity = 2f;

    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode crouchKey = KeyCode.LeftShift;

    public float jumpHeight = 1f;
    public float crouchHeight = 0f;
    public float standHeight = 2f;
    public float crouchSpeed = 1.5f;
    public bool isCrouching = false;
    private bool isDead = false;

    public Transform startPosition;
    public GameObject loadingScreen;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        currentHealth = maxHeatlh;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = characterController.isGrounded;
        if(isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Reset vertical velocity when grounded
        }
        PlayerMovement();
        CameraMovement();

        velocity.y += gravity * Time.deltaTime; // Apply gravity
        characterController.Move(velocity * Time.deltaTime);

        if(Input.GetKeyDown(jumpKey))
        {
            PlayerJump();
        }

        if (Input.GetKeyDown(crouchKey))
        {
            PlayerCrouch();
        }
    }

    void PlayerMovement()
    {
        if (GameManager.Instance.isMobile)
        {
            float currentSpeed = isCrouching ? crouchSpeed : speed;
            float moveX = VirtualJoystick.GetAxis("Horizontal") * currentSpeed;
            float moveZ = VirtualJoystick.GetAxis("Vertical") * currentSpeed;
            Vector3 move = transform.right * moveX + transform.forward * moveZ;
            characterController.Move(move * Time.deltaTime);
        }
        else
        {
            float currentSpeed = isCrouching ? crouchSpeed : speed;
            float moveX = Input.GetAxis("Horizontal") * currentSpeed;
            float moveZ = Input.GetAxis("Vertical") * currentSpeed;
            Vector3 move = transform.right * moveX + transform.forward * moveZ;
            characterController.Move(move * Time.deltaTime);
        }

    }

    void CameraMovement()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        transform.Rotate(Vector3.up * mouseX);
        float verticalLookRotation = Camera.main.transform.localEulerAngles.x - mouseY;
        Camera.main.transform.localRotation = Quaternion.Euler(verticalLookRotation, 0f, 0f);
    }

    public void PlayerJump()
    {
        if(isGrounded && !isCrouching)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); 
        }
    }

    public void PlayerCrouch()
    {
        isCrouching = !isCrouching;
        characterController.height = isCrouching ? crouchHeight : standHeight;
        characterController.radius = isCrouching ? 0.2f : 0.5f; 
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if(currentHealth <=0 && !isDead)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        GameManager.Instance.DecreaseDays();
        StartCoroutine(HandleReSpawn());
    }

    IEnumerator HandleReSpawn()
    {
        yield return new WaitForSeconds(1f);
        loadingScreen.SetActive(true);
        characterController.enabled = false;
        transform.position = startPosition.position;
        characterController.enabled = true;

        yield return new WaitForSeconds(2.5f);
        loadingScreen.SetActive(false);
        currentHealth = maxHeatlh;
        isDead = false;
    }
}
