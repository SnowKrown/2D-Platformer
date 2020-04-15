using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float bulletSpeed = 40f;

    private Rigidbody2D myRigidbody;

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Destroy(gameObject, 2f);
    }

    public void Shoot (float multiplier)
    {
        myRigidbody.AddForce(((Vector2.right * multiplier) * 100) * bulletSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 11)
            collision.gameObject.GetComponent<EnemyHealth>().MakeDamage();

        Destroy(gameObject);
    }
}
