using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public static PlayerMove instance;

    [Header("Basic Movement")]
    private float horizontalMove = 0f;
    private float horizontal;
    public float runSpeed;
    public float jumpingPower;
    private bool isFacingRight = true;
    [Space]
    [Header("Double Jump")]
    private bool doubleJump;
    public float doubleJumpingPower;
    [Space]
    [Header("Rolling")]
    public float slideSpeed;
    public float slideTime;
    public float slideCoolDown;
    public bool isSlide;
    public bool canSlide = true;
    [Space]
    public Animator animator;
    public float volume;
    private AudioSource playerAudio;
    public AudioClip jumpSound;
    public AudioClip slidingSound;
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private TrailRenderer tr;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        playerAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        HorizontalMove();

        if (Input.GetKeyDown(KeyCode.LeftShift) && canSlide == true && PlayerStatus.instance.playerStamina > 10)
        {
            StartCoroutine(Sliding());
        }

        if (IsGrounded() && !Input.GetButton("Jump"))
        {
            doubleJump = false;
            animator.SetBool("isJumping", false);

        }

        if (Input.GetButtonDown("Jump") && PlayerStatus.instance.playerStamina > 10)
        {
            if (IsGrounded() || doubleJump)
            {
                PlayerStatus.instance.StaminaBar(10);
                rb.velocity = new Vector2(rb.velocity.x, doubleJump ? doubleJumpingPower : jumpingPower);

                doubleJump = !doubleJump;
                animator.SetBool("isJumping", true);
                playerAudio.PlayOneShot(jumpSound, volume);
            }
        }

        Flip();
    }
    public void HorizontalMove()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontal * runSpeed, rb.velocity.y);
    }
    public IEnumerator Sliding()
    {
        Vector2 rollDir = new Vector2(transform.localScale.x, 0).normalized;
        Vector2 rollPosition = (Vector2)transform.position + rollDir * slideSpeed;

        isSlide = true;
        canSlide = false;
        animator.SetBool("isSlide", true);
        PlayerStatus.instance.StaminaBar(10);
        Physics2D.IgnoreLayerCollision(3, 7, true);
        Physics2D.IgnoreLayerCollision(3, 8, true);
        rb.MovePosition(rollPosition);
        tr.emitting = true;
        yield return new WaitForSeconds(slideTime);
        tr.emitting = false;
        Physics2D.IgnoreLayerCollision(3, 7, false);
        Physics2D.IgnoreLayerCollision(3, 8, false);
        playerAudio.PlayOneShot(slidingSound, volume);
        isSlide = false;
        yield return new WaitForSeconds(slideCoolDown);
        canSlide = true;
        animator.SetBool("isSlide", false);
    }
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        animator.SetBool("isJumping", false);
    }

    public void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}

