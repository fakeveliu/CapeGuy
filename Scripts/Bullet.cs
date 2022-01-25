using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.S.PlayerDestroyed();
            Destroy(collision.gameObject);
            Destroy(this);
        }
        if (collision.gameObject.tag == "PlayerAttack")
        {
            this.gameObject.SetActive(false);
            Destroy(collision.gameObject);
            Destroy(this);
        }
    }
}
