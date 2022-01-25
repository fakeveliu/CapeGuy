using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public float speed;

    private float horizontalMove = 0.0f;
    private bool jump = false;

    private Animator animator;
    public GameObject longSwipePrefab;
    public GameObject shortSwipePrefab;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.S.gameState == GameState.getReady || GameManager.S.gameState == GameState.roundWin || GameManager.S.gameState == GameState.gameOver)
            return;
        horizontalMove = Input.GetAxisRaw("Horizontal") * speed;

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("isOnGround", false);
            SoundManager.S.MakeJumpSound();
        }

        if (Input.GetKeyDown("z"))
        {
            animator.SetTrigger("isAttacking");
            ShortRangeAttack();
        }

        if (Input.GetKeyDown("x"))
        {
            animator.SetTrigger("isAttacking");
            LongRangeAttack();
        }

        // Get the approximate direction
        float ourSpeed = Input.GetAxis("Horizontal");
        animator.SetFloat("speed", Mathf.Abs(ourSpeed));

    }

    private void FixedUpdate()
    {
        if (GameManager.S.gameState == GameState.getReady || GameManager.S.gameState == GameState.roundWin)
            return;
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
        jump = false;
    }

    public void PlayerLanded()
    {
        // Debug.Log("Set onground true: " + this.tag);
        animator.SetBool("isOnGround", true);
    }

    public void LongRangeAttack()
    {
        if (GetComponent<CharacterController2D>().m_FacingRight)
        {
            GameObject swipe = Instantiate(longSwipePrefab, transform.position, Quaternion.identity);
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            swipe.transform.localScale = theScale;
            swipe.GetComponent<Swipe>().direction = Vector3.right;
            Destroy(swipe, 1.2f);
        } else
        {
            GameObject swipe = Instantiate(longSwipePrefab, transform.position, Quaternion.identity);
            swipe.GetComponent<Swipe>().direction = Vector3.left;
            Destroy(swipe, 1.2f);
        }
    }

    public void ShortRangeAttack()
    {
        Vector3 spawnPos = new Vector3();
        float offset = 2.0f;
        spawnPos = transform.position;
        if (!GetComponent<CharacterController2D>().m_FacingRight)
        {
            spawnPos.x -= offset;
            GameObject swipe = Instantiate(shortSwipePrefab, spawnPos, Quaternion.identity);
            swipe.transform.localScale *= -1;
            swipe.GetComponent<Swipe>().direction = Vector3.left;
            Destroy(swipe, 0.5f);
        }
        else
        {
            spawnPos.x += offset;
            GameObject swipe = Instantiate(shortSwipePrefab, spawnPos, Quaternion.identity);
            swipe.GetComponent<Swipe>().direction = Vector3.right;
            Destroy(swipe, 0.5f);
        }
    }
}
