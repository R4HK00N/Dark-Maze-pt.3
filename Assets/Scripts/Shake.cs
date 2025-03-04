using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public float detectionRadius = 20f; // Radius to detect monsters
    public LayerMask monsterLayer; // Layer that monsters are on
    public float maxShakeIntensity = 0.5f; // Max shake intensity (distance dependent)
    public float shakeSpeed = 10f; // Speed of the shake
    public float shakeDecay = 1f; // How fast the shake decays

    public bool canShake = true;

    private Vector3 initialPosition; // Camera's initial position
    private Vector3 shakeOffset = Vector3.zero; // Camera shake offset
    private float currentShakeIntensity = 0f; // Current shake intensity
    private Vector3 currentPosition; // Current camera position (used for shake)

    void Start()
    {
        // Store the initial position of the camera
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        // Find the closest monster
        Transform closestMonster = FindClosestMonster();

        if (closestMonster != null)
        {
            // Calculate the distance between the camera and the closest monster
            float distance = Vector3.Distance(transform.position, closestMonster.position);

            // The shake intensity increases as the monster gets closer
            currentShakeIntensity = Mathf.Clamp(maxShakeIntensity / distance, 0, maxShakeIntensity);

            if(canShake)
                ShakeCamera();
        }
        else
        {
            // If no monster is within range, return the camera to its initial position
            transform.localPosition = initialPosition;
        }
    }

    Transform FindClosestMonster()
    {
        Collider[] monstersInRange = Physics.OverlapSphere(transform.position, detectionRadius, monsterLayer);

        if (monstersInRange.Length == 0)
            return null; // No monsters within range

        Transform closestMonster = null;
        float closestDistance = Mathf.Infinity;

        // Loop through all monsters within the range and find the closest one
        foreach (Collider monsterCollider in monstersInRange)
        {
            float distance = Vector3.Distance(transform.position, monsterCollider.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestMonster = monsterCollider.transform;
            }
        }

        return closestMonster;
    }

    void ShakeCamera()
    {
        // Calculate a random offset for the shake
        shakeOffset = Random.insideUnitSphere * currentShakeIntensity;

        // Move the camera based on the shake offset, but allow normal movement
        currentPosition = initialPosition + shakeOffset;

        // Apply the shake effect
        transform.localPosition = currentPosition;

        // Decay the shake intensity over time
        currentShakeIntensity = Mathf.Lerp(currentShakeIntensity, 0, shakeDecay * Time.deltaTime);
    }
}
