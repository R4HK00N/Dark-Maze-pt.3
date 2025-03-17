using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseScreenMove : MonoBehaviour
{
    public float rotationSpeed = 5f;  // Speed of the camera rotation
    public float smoothTime = 0.1f;   // Time to smooth the stop of the camera movement

    private Vector3 currentMousePos;  // The current position of the mouse in world space
    private Vector3 targetLookAt;     // The target position the camera is looking at
    private Vector3 velocity = Vector3.zero;  // For smooth damping

    void Update()
    {
        // Step 1: Get the mouse position in the world
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            targetLookAt = hit.point;  // Set target to the point where the ray hits the surface
        }
        else
        {
            // If the ray doesn't hit anything, we could make the camera look at a default direction or point.
            targetLookAt = ray.origin + ray.direction * 10f;
        }

        // Step 2: Smoothly rotate the camera to face the target
        Vector3 directionToLookAt = targetLookAt - transform.position;
        directionToLookAt.y = 0f;  // Keep the rotation horizontal

        if (directionToLookAt.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToLookAt);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
