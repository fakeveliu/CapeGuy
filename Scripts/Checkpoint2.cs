using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint2 : MonoBehaviour
{
    public GameObject camera;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SoundManager.S.MakeCheckpointSound();
            GetComponent<EdgeCollider2D>().enabled = false;
            GameManager.S.Checkpoint(transform.position, camera.transform.position, 2);
        }
    }
}
