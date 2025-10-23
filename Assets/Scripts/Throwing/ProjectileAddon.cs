using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAddon : MonoBehaviour
{
    public int damage;
    public float destroyAfterSeconds = 3f;

    private Rigidbody rb;
    private bool targetHit;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Destroy projectile after specified time
        Destroy(gameObject, destroyAfterSeconds);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Ignore collision with player
        if (collision.gameObject.layer == LayerMask.NameToLayer("whatIsPlayer"))
            return;

        // make sure only to stick to the first target you hit
        if (targetHit)
            return;
        else
            targetHit = true;

        // check if you hit an enemy
        if (collision.gameObject.GetComponent<BasicEnemy>() != null)
        {
            BasicEnemy enemy = collision.gameObject.GetComponent<BasicEnemy>();

            enemy.TakeDamage(damage);

            // destroy projectile immediately
            Destroy(gameObject);
            return;
        }

        // make sure projectile sticks to surface
        rb.isKinematic = true;

        // make sure projectile moves with target
        transform.SetParent(collision.transform);
    }
}
