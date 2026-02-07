using UnityEngine;

public class grenadeBehav : MonoBehaviour
{
    private float damage;
    private float radius;
    public float fuseTime = 2.5f;
    public GameObject explosionEffect;

    private bool hasExploded = false;

    public void Initialize(float dmg, float rad)
    {
        damage = dmg;
        radius = rad;
        hasExploded = false; // Reset for pooling safety

        Invoke(nameof(Explode), fuseTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasExploded) return;

        // Use the Interface check instead of Tags
        IDamageable target = collision.GetComponent<IDamageable>();

        if (target != null)
        {
            Explode();
        }
    }

    void Explode()
    {
        // Double-check flag and clean up timer
        if (hasExploded) return;
        hasExploded = true;
        CancelInvoke(nameof(Explode));

        // Detect all IDamageable objects in range
        Collider2D[] objectsInRange = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (Collider2D obj in objectsInRange)
        {
            IDamageable target = obj.GetComponent<IDamageable>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }

            // Optional: Physics push for anything with a Rigidbody
            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 dir = (Vector2)obj.transform.position - (Vector2)transform.position;
                rb.AddForce(dir.normalized * 5f, ForceMode2D.Impulse);
            }
        }

        // Visuals and Cleanup
        if (explosionEffect) Instantiate(explosionEffect, transform.position, Quaternion.identity);

        // If you use Object Pooling, use gameObject.SetActive(false) instead
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}