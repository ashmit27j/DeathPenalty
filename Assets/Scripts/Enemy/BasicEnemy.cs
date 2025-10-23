using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    [Header("Stats")]
    public int health;

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
            DestroyEnemy();
    }

    public void DestroyEnemy()
    {
        // On death, increase shuriken count by 1
        if (ThrowingTutorial.instance != null)
        {
            ThrowingTutorial.instance.AddShuriken(1);
        }

        Destroy(gameObject);
    }
}
