using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marsh : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.S.PlayerDestroyed();
            SoundManager.S.MakePlayerDeathSound();
            Destroy(collision.gameObject);
        }
    }
}
