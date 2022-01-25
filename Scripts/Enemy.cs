using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public bool faceLeft = true;

    public GameObject bulletPrefab;

    private CharacterController2D controller;

    private int livesStart = 10;
    private int livesLeft;

    public bool alive;

    private void Start()
    {
        controller = GetComponent<CharacterController2D>();
        alive = true;
        livesLeft = livesStart;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontalMove = speed * Time.fixedDeltaTime;

        if (!alive) { StopAllCoroutines(); }

        if (faceLeft) { horizontalMove *= -1.0f; }
        controller.Move(horizontalMove, false, false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "TurnAround")
        {
            faceLeft = !faceLeft;
        } else if (collision.gameObject.tag == "PlayerAttack")
        {
            StartCoroutine(Blink());
            livesLeft--;
            collision.gameObject.GetComponent<Swipe>().speed *= 0.2f;
            Destroy(collision.gameObject, 0.5f);
            if (livesLeft <= 0)
            {
                alive = false;
                SoundManager.S.MakeEnemyDeathSound();
                GameManager.S.EnemyDestroyed();
                GetComponent<BoxCollider2D>().enabled = false;
                GetComponent<CircleCollider2D>().enabled = false;
                StopAllCoroutines();
                Destroy(this.gameObject, 1.0f);
                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                rb.bodyType = RigidbodyType2D.Kinematic;
                GetComponent<Animator>().SetTrigger("Dying");
                speed = 0;
            }
        }
        
    }

    private IEnumerator Blink()
    {
        GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.2f, 0.2f, 1.0f);
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.2f, 0.2f, 1.0f);
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    public IEnumerator Shoot()
    {
        yield return new WaitForSeconds(2.0f);

        while (GameManager.S.gameState == GameState.playing && alive)
        {
                float timeBetweenBullets = 2.0f;
                
                if (faceLeft)
                {
                    GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 0, 90));
                    bullet.GetComponent<Bullet>().direction = Vector3.left;
                    Destroy(bullet, 2.0f);
                }
                else
                {
                    GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 0, 90));
                    bullet.GetComponent<Bullet>().direction = Vector3.right;
                    Destroy(bullet, 2.0f);
            }

            yield return new WaitForSeconds(timeBetweenBullets);
      
        }

    }

}
