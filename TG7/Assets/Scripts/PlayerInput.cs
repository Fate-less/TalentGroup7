using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float currentSpeed = 5f;
    [SerializeField] float acceleration = 5f;
    [SerializeField] float jumpHeight = 10f;
    [SerializeField] float jumpGravity = 1;
    [SerializeField] float fallGravity = 2;
    [SerializeField] float deathDelay = 3f;
    [SerializeField] Vector2 groundCheckOffset;
    [SerializeField] Vector2 groundCheckSize;

    private Rigidbody2D rb;
    private bool isGrounded;
    private TMP_InputField inputField;
    private string moveState;
    private Vector3 spawnPos;
    private bool freeze = false;
    private float horizontalInput;
    private float jumpForce;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spawnPos = gameObject.transform.position;
        inputField = GameObject.Find("InputField").GetComponent<TMP_InputField>();
        inputField.Select();
    }

    private void Update()
    {
        GroundCheck();
        currentSpeed = Mathf.MoveTowards(currentSpeed, moveSpeed * horizontalInput, acceleration * Time.deltaTime);
        if ((moveState == "left" && !freeze) || (moveState == "right" && !freeze))
        {
            MovePlayer();
        }
        else if (moveState == "stop" && !freeze)
        {
            StopMoving();
        }
        if(rb.velocity.y > 0)
        {
            rb.gravityScale = jumpGravity;
        }
        else
        {
            rb.gravityScale = fallGravity;
        }
    }
    public void InputWord(string word)
    {
        switch (word)
        {
            case "left":
                currentSpeed = 0;
                moveState = "left";
                horizontalInput = -1;
                break;
            case "right":
                currentSpeed = 0;
                moveState = "right";
                horizontalInput = 1;
                break;
            case "jump":
                Jump();
                break;
            case "stop":
                currentSpeed = 0;
                moveState = "stop";
                break;
        }
        ClearInput();
        inputField.ActivateInputField();
    }

    public void ClearInput()
    {
        inputField.text = ("");
    }

    private void MovePlayer()
    {
        rb.velocity = new Vector2(currentSpeed, rb.velocity.y);
    }

    private void Jump()
    {
        if (isGrounded)
        {
            jumpForce = Mathf.Sqrt(2f * jumpHeight * jumpGravity);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false;
        }
    }

    private void StopMoving()
    {
        rb.velocity = new Vector2(0f, rb.velocity.y);
    }

    private void GroundCheck()
    {
        Collider2D collider = Physics2D.OverlapBox((Vector2)transform.position + groundCheckOffset, groundCheckSize, 0f);
        isGrounded = (collider != null && (collider.gameObject.CompareTag("Ground") || (rb.velocity.y <= .1f && collider.gameObject.CompareTag("Platform"))));
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            freeze = true;
            moveState = null;
            StartCoroutine(Timer());
        }
        if (isGrounded && collision.gameObject.CompareTag("Door"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(deathDelay);
        gameObject.transform.position = spawnPos;
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        freeze = false;
    }
}

