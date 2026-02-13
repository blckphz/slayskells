using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float damage;

    public void SetDamage(float dmg)
    {
        damage = dmg;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Only damage objects that have an enemyHealth component
        if (collider.TryGetComponent<enemyHealth>(out enemyHealth enemy))
        {
            enemy.TakeDamage(damage);
            Debug.Log($"[Projectile] Hit {enemy.name} for {damage} damage");
            Destroy(gameObject);
        }
    }
}
