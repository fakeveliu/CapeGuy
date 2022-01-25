using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCoinScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
            SoundManager.S.MakeLevelSound();
            GameManager.S.EndOfLevel();
        }
    }
}
