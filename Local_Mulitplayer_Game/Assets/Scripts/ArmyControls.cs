using System.Collections;
using UnityEngine;

public class ArmyControls : MonoBehaviour
{
    public float health = 100f;
    public float damage = 30f;
    private Animator animator;
    public float damagePerSecond = 10f;
    public Collider armyCollider;

    private bool isDead = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        //TakeDamage(damagePerSecond * Time.deltaTime);
        //Debug.Log($"Health is now {health}.");
    }

    // Take damage
    public void TakeDamage(float amount)
    {
        if (isDead) return;

        health -= amount;
        Debug.Log($"{gameObject.name} took {amount} damage. Health: {health}");

        if (health <= 0)
        {
            onDeath();
        }
    }

    public void onDeath()
    {
        if (!isDead)
        {
            isDead = true;
            animator.SetBool("isDead", true);
            Debug.Log($"{gameObject.name} has died.");

            armyCollider.enabled = false;
            StartCoroutine(DestroyAfterAnimation());
           
        }
    }

    // Delay death to finish the animation 
    private IEnumerator DestroyAfterAnimation()
    {
        AnimatorStateInfo dead = animator.GetCurrentAnimatorStateInfo(1);
        yield return new WaitForSeconds(dead.length);

        Destroy(gameObject,10f);
    }
}
