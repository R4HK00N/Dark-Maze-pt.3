using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float bobbingAmount = 0.05f; // How much bobbing effect is applied
    public float bobbingMultiplier = 1f;

    private float timer = 0f;
    private bool isWalking;
    private bool isSprinting;

    public float walkSpeed = 5f; // Speed when walking
    public float sprintSpeed = 10f; // Speed when sprinting
    public float mouseSensitivity = 2f; // Camera mouse sensitivity
    public Transform orientation; // The orientation transform to rotate the camera

    public AudioSource walkingSFX;
    public float soundEffectDelay;

    private Rigidbody rb;
    private float currentSpeed;
    private float xRotation = 0f;

    public bool walkinSoundactive;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Disable rotation on Rigidbody to control it manually
    }

    private void Update()
    {
        HandleMovement();

        isWalking = (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) && Camera.main.transform.localPosition.y != 0f);

        isSprinting = (isWalking && Input.GetKey(KeyCode.LeftShift));

        // Handle camera input (Looking around)
        HandleCameraRotation();

        // Apply bobbing effect when walking
        if (isWalking)
        {
            ApplyBobbing();
            if (!walkinSoundactive)
            {
                walkinSoundactive = true;
                walkingSFX.Play();
                StartCoroutine(SoundEffectDelay(soundEffectDelay));
            }
        }
        else
        {
            // Reset bobbing when not moving
            timer = 0f;
            Camera.main.transform.localPosition = new Vector3(Camera.main.transform.localPosition.x, 0f, Camera.main.transform.localPosition.z);

            walkinSoundactive = false;
            walkingSFX.Pause();
        }
    }

    IEnumerator SoundEffectDelay(float time)
    {
        yield return new WaitForSeconds(time);
        walkinSoundactive = false;
        walkingSFX.Stop();
    }

    private void ApplyBobbing()
    {
        float currentBobbingAmount = isSprinting ? bobbingAmount * bobbingMultiplier : bobbingAmount;
        float currentBobbingSpeed = isSprinting ? sprintSpeed : walkSpeed;

        // Bobbing based on the movement timer
        timer += Time.deltaTime * currentBobbingSpeed * bobbingMultiplier;

        // Apply the bobbing effect
        float bobbingPositionY = Mathf.Sin(timer) * currentBobbingAmount * bobbingMultiplier;

        // Adjust the local position of the camera for bobbing
        Camera.main.transform.localPosition = new Vector3(Camera.main.transform.localPosition.x, bobbingPositionY, Camera.main.transform.localPosition.z);
    }

    private void HandleMovement()
    {
        // Input for horizontal and vertical movement
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Adjust the movement direction based on the camera's orientation
        Vector3 moveDirection = orientation.forward * vertical + orientation.right * horizontal;

        // Normalize the moveDirection so diagonal movement doesn't cause faster speed
        moveDirection.Normalize();

        // Determine movement speed based on sprinting
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = sprintSpeed;
            walkingSFX.pitch = 1f;
        }
        else
        {
            currentSpeed = walkSpeed;
            walkingSFX.pitch = 0.65f;
        }

        // Apply movement using Rigidbody
        Vector3 moveVelocity = moveDirection * currentSpeed;

        // Only apply velocity on the X and Z axes (no Y velocity in this function)
        Vector3 targetVelocity = new Vector3(moveVelocity.x, rb.velocity.y, moveVelocity.z);
        rb.velocity = targetVelocity;
    }

    private void HandleCameraRotation()
    {
        // Mouse input for rotating the camera
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

        // Rotate player horizontally
        transform.Rotate(Vector3.up * mouseX);

        // Rotate camera vertically (clamping the up/down rotation)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Prevent camera flipping
        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
