using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemScript : MonoBehaviour
{
    public AudioSource gemSound;
    public float audioTime;
    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            gemSound.Play();

            Destroy(gameObject);
        }
    }
}
