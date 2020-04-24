using CommandTerminal;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class BoolEvent : UnityEvent<bool> { }

public class PlayerMovement : MonoBehaviour
{
    [SerializeField, Header("Events")]
    private UnityEvent OnLandEvent = default;
    [SerializeField]
    private BoolEvent OnGroundedChangeEvent = default;
    [SerializeField, Space, Header("Jump Properties")]
    private float jumpForce = 5;
    [SerializeField]
    private float jumpRate = 0.1F;
    [SerializeField]
    private float edgeTimeJump = 0.2F;
    [SerializeField]
    private int maxJumps = 2;
    [SerializeField]
    private AudioClip[] jumpClips = default;
    [SerializeField, Header("SlowMo Jump")]
    private int jumpsToSlowMo = -1;
    [SerializeField, Range(0, 1)]
    private float jumpSlowMoScale = 0.35F;
    [SerializeField]
    private float jumpSlowMoDuration = 0.5F;
    [SerializeField, Range(0, .3f), Header("Other Properties")]
    private float movementSmoothing = .05f;
    [SerializeField]
    private bool airControl = false;
    [SerializeField]
    private LayerMask groundLayer = default;
    [SerializeField]
    private Transform groundCheck = default;
    [SerializeField]
    private Animator animator = default;

    private TimeScaler timeScaler;
    private AudioSource jumpSource;
    private float groundedRadius = 0.1f;
    private bool isGrounded = true;
    private Rigidbody2D myRigidbody2D;
    private bool isFacingRight = true;
    private Vector3 moveVelocity = Vector3.zero;
    private int currentJumps = 0;
    private float airTime = 0;
    private float nextJump = 0;
    private float initialGravity = 0;
    public bool IsGrounded => isGrounded;

    private void Awake()
    {
        timeScaler = FindObjectOfType<TimeScaler>();
        jumpSource = gameObject.AddComponent<AudioSource>();
        myRigidbody2D = GetComponent<Rigidbody2D>();
        initialGravity = myRigidbody2D.gravityScale;
        Terminal.Shell.AddCommand("Player_SetGravity", SetGravity, 1, 1, "Set the player gravity. Args: New gravity (Float).");
        Terminal.Shell.AddCommand("Player_ResetGravity", ResetGravity, 0, 0, "Reset the player gravity.");
    }

    private void FixedUpdate()
    {
        bool wasGrounded = isGrounded;
        isGrounded = false;
        CheckIsGrounded(wasGrounded);

        if (!isGrounded)
            airTime += Time.fixedDeltaTime;
        else
        {
            airTime = 0;
            currentJumps = 0;
        }
    }

    private void CheckIsGrounded(bool wasGrounded)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, groundLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                isGrounded = true;
                
                if (!wasGrounded && myRigidbody2D.velocity.y < -0.1F)
                    OnLanded();
            }
        }

        if (isGrounded != wasGrounded)
            OnGroundedChangeEvent.Invoke(isGrounded);

        animator.SetBool("IsGrounded", isGrounded);
    }

    private void OnLanded()
    {
        OnLandEvent.Invoke();
        airTime = 0;
    }

    public void TryMove(float move, bool shouldBeJump)
    {
        if (isGrounded || airControl)
            Move(move);

        if (shouldBeJump)
            TryJump();
    }

    private void Move(float move)
    {
        Vector3 targetVelocity = new Vector2(move * 10f, myRigidbody2D.velocity.y);
        myRigidbody2D.velocity = Vector3.SmoothDamp(myRigidbody2D.velocity, targetVelocity, ref moveVelocity, movementSmoothing);
        TryFlipPlayer(move);
    }

    private void TryFlipPlayer(float move)
    {
        if (move > 0 && !isFacingRight)
            Flip();
        else if (move < 0 && isFacingRight)
            Flip();
    }

    private void TryJump()
    {
        if (Time.time > nextJump && currentJumps < maxJumps)
        {
            if (currentJumps < 1 && airTime < edgeTimeJump)
                Jump();
            else if (currentJumps > 0)
                Jump();

            CancelInvoke(nameof(IncreaseCurrentJump));
            Invoke(nameof(IncreaseCurrentJump), jumpRate / 2);
            nextJump = Time.time + jumpRate;
        }
    }

    private void IncreaseCurrentJump()
    {
        currentJumps++;
    }

    private void Jump()
    {
        myRigidbody2D.velocity = Vector2.up * jumpForce;
        JumpSound();
        TrySlowMo();

        if (airTime > edgeTimeJump)
            currentJumps = maxJumps;
    }

    private void JumpSound()
    {
        jumpSource.clip = jumpClips[Random.Range(0, jumpClips.Length)];
        if (currentJumps < 1)
            jumpSource.pitch = Random.Range(0.9F, 1.1F);
        else
            jumpSource.pitch += 0.25F;

        jumpSource.Play();
    }

    private void TrySlowMo()
    {
        if (jumpsToSlowMo < 0 || currentJumps < jumpsToSlowMo)
            return;

        timeScaler.ScaleTo(jumpSlowMoScale, jumpSlowMoDuration);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void SetGravity(CommandArg[] args)
    {
        if (Terminal.IssuedError) return;

        float newGravity = args[0].Float;
        myRigidbody2D.gravityScale = newGravity;
    }

    private void ResetGravity(CommandArg[] args)
    {
        if (Terminal.IssuedError) return;
        myRigidbody2D.gravityScale = initialGravity;
    }
}