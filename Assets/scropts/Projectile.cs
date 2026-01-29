using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damageValue = 25f;
    public float lifetime = 3f;

    void Start()
    {
        // Destroy bullet after X seconds so it doesn't fly forever
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Try to find the interface on the object we hit
        IDamageable damageable = collision.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(damageValue);
            Destroy(gameObject); // Destroy bullet on hit
        }
    }
}