using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SoundManager.S.MakePowerUpSound();
            GameManager.S.PoweredUp();
            Destroy(this.gameObject);
        }
    }
}
