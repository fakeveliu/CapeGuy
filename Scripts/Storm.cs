using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storm : MonoBehaviour
{
    public float speed;
    public Vector3 direction = new Vector3();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += direction * speed * Time.fixedDeltaTime;
        if (GameManager.S.gameState == GameState.oops || GameManager.S.gameState == GameState.gameOver)
        {
            GetComponent<PolygonCollider2D>().enabled = false;
            Destroy(this.gameObject, 1.0f);
        }
    }

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
