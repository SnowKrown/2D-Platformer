using UnityEngine;

public class BetterJump : MonoBehaviour
{
    [SerializeField] private float fallMultiplier = 2.5F;
    [SerializeField] private float lowJumpMultiplier = 2.0F;

    private Rigidbody2D rBody;
    private PlayerController pController;

    private void Awake()
    {
        rBody = GetComponent<Rigidbody2D>();
        pController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (rBody.velocity.y < 0)
            rBody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        else if (rBody.velocity.y > 0 && !pController.IsPressingJump)
            rBody.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
    }
}
