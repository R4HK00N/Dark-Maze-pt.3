using UnityEngine;

public class SmoothCameraLookAt : MonoBehaviour
{
    public float rotationSpeed = 2f;   // Speed of the camera rotation (adjustable)
    public float smoothStopSpeed = 1f; // Speed at which the camera stops moving when the mouse stops
    public float minPitch = -60f;      // Minimum vertical (pitch) rotation in degrees
    public float maxPitch = 60f;       // Maximum vertical (pitch) rotation in degrees
    public float minYaw = -90f;        // Minimum horizontal (yaw) rotation in degrees
    public float maxYaw = 90f;         // Maximum horizontal (yaw) rotation in degrees

    private Vector3 lastMousePos;      // Last position of the mouse
    private Vector3 currentMousePos;   // Current position of the mouse
    private bool isMouseMoving = false; // Flag to track if the mouse is moving
    private Quaternion targetRotation; // Target rotation to smoothly move towards
    private float stopTime = 0f;       // Time passed since the mouse stopped moving, used for smooth stop
    private float currentYaw = 0f;     // Current yaw (horizontal) rotation in degrees
    private float currentPitch = 0f;   // Current pitch (vertical) rotation in degrees

    void Update()
    {
        // Step 1: Get the current mouse position
        currentMousePos = Input.mousePosition;

        // Step 2: Check if the mouse is moving
        if (currentMousePos != lastMousePos)
        {
            isMouseMoving = true;
            stopTime = 0f;  // Reset stop time when mouse starts moving
        }
        else
        {
            isMouseMoving = false;
        }

        // Step 3: If the mouse is moving, update the target rotation
        if (isMouseMoving)
        {
            // Calculate direction based on mouse position
            Ray ray = Camera.main.ScreenPointToRay(currentMousePos);
            Vector3 targetLookAt = ray.GetPoint(10); // We pick a point at distance 10 for the direction

            // Calculate the direction to look at
            Vector3 directionToLookAt = targetLookAt - transform.position;

            // Get the angles of the rotation
            float targetYaw = Mathf.Atan2(directionToLookAt.x, directionToLookAt.z) * Mathf.Rad2Deg;
            float targetPitch = Mathf.Asin(directionToLookAt.y / directionToLookAt.magnitude) * Mathf.Rad2Deg;

            // Invert the pitch to fix the up/down axis inversion
            targetPitch = -targetPitch;

            // Clamp pitch to prevent flipping
            targetPitch = Mathf.Clamp(targetPitch, minPitch, maxPitch);

            // Clamp yaw to prevent 360 rotation
            targetYaw = Mathf.Clamp(targetYaw, minYaw, maxYaw);

            // Update the target rotation (convert angles to Quaternion)
            targetRotation = Quaternion.Euler(targetPitch, targetYaw, 0f);
        }
        else
        {
            // When the mouse stops, gradually slow down the rotation
            stopTime += Time.deltaTime; // Increase time passed since the mouse stopped
        }

        // Step 4: Smoothly apply the target rotation
        if (isMouseMoving)
        {
            // While the mouse is moving, smoothly rotate towards the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            // While the mouse is not moving, slowly stop the rotation
            // Interpolate towards the last target rotation to make the stop smooth
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Mathf.Exp(-smoothStopSpeed * stopTime) * Time.deltaTime);
        }

        // Step 5: Store the current mouse position for the next frame
        lastMousePos = currentMousePos;
    }
}
