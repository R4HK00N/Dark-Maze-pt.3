using UnityEngine;

public class CandleFlicker : MonoBehaviour
{
    public Light candleLight;      // Reference to the point light
    public float minIntensity = 0.5f; // Minimum light intensity
    public float maxIntensity = 1.5f; // Maximum light intensity
    public float flickerSpeed = 0.1f; // Speed of the flicker effect
    public float flickerVariation = 0.1f; // How much variation in intensity per flicker

    private float targetIntensity;

    void Start()
    {
        if (candleLight == null)
        {
            candleLight = GetComponent<Light>(); // Ensure the script finds the light component if not set
        }

        // Set the initial target intensity
        targetIntensity = candleLight.intensity;
    }

    void Update()
    {
        // Flicker the light by changing its intensity randomly
        targetIntensity = Mathf.Lerp(minIntensity, maxIntensity, Random.Range(0f, 1f));

        // Smoothly transition towards the target intensity
        candleLight.intensity = Mathf.Lerp(candleLight.intensity, targetIntensity, flickerSpeed * Time.deltaTime);

        // Optionally add some color flicker to mimic candlelight (yellowish tint)
        //candleLight.color = new Color(1f, 0.9f, 0.6f) + new Color(Random.Range(-flickerVariation, flickerVariation), Random.Range(-flickerVariation, flickerVariation), Random.Range(-flickerVariation, flickerVariation));
    }
}

