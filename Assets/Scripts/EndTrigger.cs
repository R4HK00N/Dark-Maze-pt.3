using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    public GameObject player;
    public GameObject winscreen;
    public GameManager manager;
    public void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            winscreen.SetActive(true);

            player.GetComponent<Movement>().enabled = false;
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            manager.StopEnemies();

            Cursor.lockState = CursorLockMode.None;
        }
    }
}
