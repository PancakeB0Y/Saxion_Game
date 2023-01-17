using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CameraSwitcher;

public class PlayerMovement : MonoBehaviour
{
    Transform mainCamera;
    [SerializeField] CharacterController controller;

    [SerializeField] float speed = 10f;
    [SerializeField] float jumpImpulse = 5f;
    [SerializeField] float gravity = -19.62f;

    [SerializeField] float turnSmoothTime = 0.1f;
    float smoothTurnVelocity;

    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundDistance = 0.1f;
    bool isGrounded;

    public Vector3 velocity;

    public static int minigamesWon = 0;
    
    private void Start()
    {
        mainCamera = Camera.main.transform;
        Cursor.visible = false;
    }
    void Update()
    {
        if (isInMinigame) {
            return;
        }

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothTurnVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDirection * speed * Time.deltaTime);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = jumpImpulse;
        }
    }
}
