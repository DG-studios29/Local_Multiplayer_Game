using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;
    private GameObject shooter; // Track who fired this projectile

    // Set damage through the HeroAbility when the projectile is spawned
    public void Initialize(GameObject owner, int damageAmount)
    {
        shooter = owner;
        damage = damageAmount; // Set the damage based on the ability
    }

    public void SetShooter(GameObject owner)
    {
        shooter = owner;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == shooter)
            return; // Prevent self-damage

        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>()?.TakeDamage(damage);
            collision.gameObject.GetComponent<EnemyAI>()?.TakeDamage(damage);
            Destroy(gameObject); // Destroy on impact
        }
        else
        {
            Destroy(gameObject, 5f); // Destroy after 5s if no impact
        }
    }
}
