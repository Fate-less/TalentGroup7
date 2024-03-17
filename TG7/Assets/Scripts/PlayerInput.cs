using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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
    [SerializeField] float deathDelay = 3f, goalDelay = 1f;
    [SerializeField] UnityEvent OnReachGoal;
    [SerializeField] Vector2 groundCheckOffset;
    [SerializeField] Vector2 groundCheckSize;
    [SerializeField] AudioClip[] walkSounds;
    [SerializeField] AudioClip deathSound;
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip doorOpenSound;
    [SerializeField] AudioClip doorClosedSound;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;
    private bool isGrounded;
    private TMP_InputField inputField;
    private string moveState;
    private Vector3 spawnPos;
    private bool freeze = false;
    private float horizontalInput;
    private float jumpForce;
    private AudioSource audioSource;
    private SaveLevel saveLevel;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        saveLevel = GetComponent<SaveLevel>();
        audioSource = GetComponent<AudioSource>();
        spawnPos = gameObject.transform.position;
        inputField = GameObject.Find("InputField").GetComponent<TMP_InputField>();
        inputField.Select();
    }

    private void Update()
    {
        GroundCheck();
        Animate();
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
        ClearInput();
        inputField.ActivateInputField();
        if(freeze)
            return;
        
        switch (word)
        {
            case "left":
                currentSpeed = 0;
                moveState = "left";
                sr.flipX = true;
                horizontalInput = -1;
                break;
            case "right":
                currentSpeed = 0;
                moveState = "right";
                sr.flipX = false;
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
    }

    public void ClearInput()
    {
        inputField.text = ("");
    }

    private void MovePlayer()
    {
        rb.velocity = new Vector2(currentSpeed, rb.velocity.y);
        if (!audioSource.isPlaying)
        {
            WalkSound();
        }
    }

    private void Jump()
    {
        if (isGrounded)
        {
            audioSource.PlayOneShot(jumpSound);
            jumpForce = Mathf.Sqrt(2f * jumpHeight * jumpGravity);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false;
        }
    }

    private void StopMoving()
    {
        rb.velocity = new Vector2(0f, rb.velocity.y);
        audioSource.Stop();
    }

    private void GroundCheck()
    {
        Collider2D collider = Physics2D.OverlapBox((Vector2)transform.position + groundCheckOffset, groundCheckSize, 0f);
        isGrounded = (collider != null && (collider.gameObject.CompareTag("Ground") || (rb.velocity.y <= .1f && collider.gameObject.CompareTag("Platform"))));
    }

    private void Animate()
    {
        if(freeze)
        {
            anim.Play("PlayerDeath");
            return;
        }
        if(!isGrounded)
        {
            anim.Play(rb.velocity.y > 0f?"PlayerJump":"PlayerMidair");
            return;
        }
        anim.Play(moveState == "stop" || moveState == null?"PlayerIdle":"PlayerWalk");
    }

    bool hasReachedGoal = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!freeze && collision.gameObject.CompareTag("Obstacle"))
        {
            audioSource.Stop();
            audioSource.PlayOneShot(deathSound);
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            freeze = true;
            StartCoroutine(Timer());
        }
        if(!hasReachedGoal && collision.gameObject.CompareTag("Door"))
        {
            audioSource.Stop();
            audioSource.PlayOneShot(doorOpenSound);
            moveState = "stop";
            if (isGrounded)
            {
                hasReachedGoal = true;
                StartCoroutine(ReachGoal());
            }
        }
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(deathDelay);
        Respawn();
    }

    IEnumerator ReachGoal()
    {
        OnReachGoal.Invoke();
        yield return new WaitForSeconds(goalDelay);
        audioSource.PlayOneShot(doorOpenSound);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        saveLevel.SaveGame(saveLevel.LoadGame() + 1);
    }

    private void Respawn()
    {
        gameObject.transform.position = spawnPos;
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.velocity = Vector2.zero;
        freeze = false;
        moveState = null;
        sr.flipX = false;
        ClearInput();
    }

    public void WalkSound()
    {
        AudioClip randomClip = walkSounds[Random.Range(0, walkSounds.Length)];
        audioSource.clip = randomClip;
        audioSource.Play();
    }
}

