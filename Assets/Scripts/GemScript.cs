using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemScript : MonoBehaviour
{
    public AudioSource gemSound;
    public float audioTime;
    float randomPitchValue;
    public void Start()
    {
        randomPitchValue = Random.Range(0.8f, 1.2f);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            gemSound.pitch = randomPitchValue;
            gemSound.Play();

            Destroy(gameObject);
        }
    }
}
