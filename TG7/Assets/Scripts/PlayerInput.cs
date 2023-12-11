using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private TMP_InputField inputField;
    private string moveState;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        inputField = GameObject.Find("InputField").GetComponent<TMP_InputField>();
        inputField.Select();
    }

    private void Update()
    {
        if (moveState == "left")
        {
            MoveLeft();
        }
        else if (moveState == "right")
        {
            MoveRight();
        }
        else if (moveState == "stop")
        {
            StopMoving();
        }
    }
    public void Input(string word)
    {
        switch (word)
        {
            case "left":
                moveState = "left";
                break;
            case "right":
                moveState = "right";
                break;
            case "jump":
                Jump();
                break;
            case "stop":
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

    private void MoveLeft()
    {
        rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
    }

    private void MoveRight()
    {
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
    }

    private void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false;
        }
    }

    private void StopMoving()
    {
        rb.velocity = new Vector2(0f, rb.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player is grounded
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}

