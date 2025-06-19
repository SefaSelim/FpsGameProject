using UnityEngine;

public class CharacterControllers : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = -9.81f;

    private float verticalVelocity = 0f;

    CharacterController characterController;
    Camera mainCamera;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        Vector3 moveInput = new Vector3(
            Input.GetAxis("Horizontal"),
            0f,
            Input.GetAxis("Vertical")
        );

        if (moveInput != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
            moveInput *= 2f;

        // Zıplama
        if (characterController.isGrounded)
        {
            verticalVelocity = -1f; // yere bastığında hafif aşağıya çek
            if (Input.GetButtonDown("Jump"))
                verticalVelocity = Mathf.Sqrt(jumpForce * -2f * gravity);
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        // Hareket uygulaması
        Vector3 moveDirection = transform.forward * speed * moveInput.z +
                                transform.right * speed * moveInput.x +
                                Vector3.up * verticalVelocity;

        characterController.Move(moveDirection * Time.deltaTime);

        // Kamera yönüne bak
        Vector3 lookDirection = mainCamera.transform.forward;
        lookDirection.y = 0;
        transform.forward = lookDirection;
        
        
    }
    
}