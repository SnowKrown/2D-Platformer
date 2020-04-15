using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private int maxHp = default;
    //[SerializeField]
    //private Animator animator = default;
    //[SerializeField]
    //private GameObject dieParticles = default;
    [SerializeField]
    private Transform initialPoint = default;
    [SerializeField]
    private Rigidbody2D myRigidbody = default;
    [SerializeField]
    private GameObject spawnParticles = default;

    private int currentHp;

    public bool IsDead
    {
        get { return currentHp <= 0; }
    }

    private void Awake()
    {
        //dieParticles.SetActive(false);
        currentHp = maxHp;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 11)
            MakeDamage();
    }

    private void MakeDamage()
    {
        if (IsDead)
            return;

        currentHp--;

        if (IsDead)
        {
            myRigidbody.simulated = false;
            //animator.SetTrigger("Die");
            //dieParticles.SetActive(true);
            Invoke(nameof(Respawn), 3);
        }
    }

    private void Respawn()
    {
        myRigidbody.simulated = true;
        transform.position = initialPoint.position;
        currentHp = maxHp;
        //animator.Play("cat_idle");
        spawnParticles.SetActive(true);
    }

    public void OverrideInitialPoint(Transform nextPoint)
    {
        initialPoint = nextPoint;
    }
}
