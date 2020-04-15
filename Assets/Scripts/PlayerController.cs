using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float jumpPressedRememberTime = 0.2f;
    [SerializeField]
    private PlayerHealth playerHealth = default;

    public PlayerMovement controller;
    public Animator animator;

    public float runSpeed = 40f;

    private float nextJump = 0;
    private float horizontalMove = 0f;
    private bool jump = false;

    public bool IsPressingJump { get; private set; }

    private void Update()
    {
        if (playerHealth.IsDead)
            return;

        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        IsPressingJump = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        bool isPressingJumpDown = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);

        nextJump -= Time.deltaTime;

        if (isPressingJumpDown)
            nextJump = jumpPressedRememberTime;

        if (nextJump > 0)
        {
            nextJump = 0;
            jump = true;
        }
    }
    
    private void FixedUpdate()
    {
        controller.TryMove(horizontalMove * Time.fixedDeltaTime, jump);
        jump = false;
    }

    public void OnShoot()
    {
        animator.SetTrigger("Shoot");
    }
}