using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Boss : MonoBehaviour
{
    private float healthStart = 500f;
    private float healthLeft;
    public GameObject HealthBarObject;
    private BossHealthBar HPBar;

    public TextMeshProUGUI messageCrit;

    public GameObject stormPrefab;
    public GameObject laserPrefab;
    public GameObject player;

    private bool alive;

    // Start is called before the first frame update
    void Start()
    {
        healthLeft = healthStart;
        HPBar = HealthBarObject.GetComponent<BossHealthBar>();
        alive = true;
        messageCrit.enabled = false;
    }

    void Update()
    {
        if (!alive)
            StopAllCoroutines();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerAttack")
        {
            StartCoroutine(DealDamage());
            HPBar.SetHealthBarValue(healthLeft / healthStart);
            collision.gameObject.GetComponent<Swipe>().speed *= 0.2f;
            Destroy(collision.gameObject, 0.5f);
            SoundManager.S.MakeEnemyDeathSound();
            if (healthLeft <= 0)
            {
                alive = false;
                GetComponent<PolygonCollider2D>().enabled = false;
                GameManager.S.EnemyDestroyed();
                GetComponent<Animator>().SetTrigger("isDying");
                Destroy(this.gameObject, 2.0f);
                GameManager.S.GameOverWin();
            }
        }

    }

    private IEnumerator DealDamage()
    {
        float crit = Random.Range(0.0f, 1.0f);
        if (crit < 0.1f)
        {
            healthLeft -= 50f;
            messageCrit.enabled = true;
            yield return new WaitForSeconds(0.2f);
            messageCrit.enabled = false;
        } else
        {
            healthLeft -= 20f;
        }
        GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.2f, 0.2f, 1.0f);
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.2f, 0.2f, 1.0f);
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    public IEnumerator Attack()
    {
        yield return new WaitForSeconds(2.0f);

        while (GameManager.S.gameState == GameState.playing)
        {
            GetComponent<Animator>().SetTrigger("isAttacking");
            float timeBetweenAttacks = 3.0f;

            float chooseAttack = Random.Range(0.0f, 1.0f);
            if (chooseAttack < 0.3f)
            {
                yield return new WaitForSeconds(0.5f);
                StormAttack();
            } else { // laser attack
                Vector3 relative = transform.InverseTransformPoint(player.transform.position);
                float angle = Mathf.Atan2(relative.y, relative.x) * Mathf.Rad2Deg - 180;

                GameObject laserObject = Instantiate(laserPrefab, transform.position, Quaternion.Euler(0, 0, angle));
                yield return new WaitForSeconds(1.5f);
                SoundManager.S.MakeLaserSound();
                if (!laserObject)
                    break;
                GameObject laser = laserObject.transform.GetChild(0).gameObject;
                laser.GetComponent<Animator>().SetTrigger("isAttacking");
                laserObject.GetComponent<BoxCollider2D>().enabled = true;
                Destroy(laserObject, 1.0f);
            }

            yield return new WaitForSeconds(timeBetweenAttacks);

        }
    }

    private void StormAttack()
    {
        Vector3 spawnPos = new Vector3();
        spawnPos = transform.position;
        spawnPos.y += 0.3f;

        GameObject storm = Instantiate(stormPrefab, spawnPos, Quaternion.identity);
        storm.GetComponent<Storm>().direction = Vector3.left;
        Destroy(storm, 5.0f);
    }

    public void ResetHealth()
    {
        healthLeft = healthStart;
        HPBar.SetHealthBarValue(1.0f);
    }
}
