using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    public float health = 100f;
    public GameObject damageTextPrefab; // Drag your TMP prefab here in Inspector

    public void TakeDamage(float damage)
    {
        health -= damage;

        // Spawn the damage number at the enemy's position
        ShowDamageText(damage);

        if (health <= 0) Die();
    }

    void ShowDamageText(float damage)
    {
        if (damageTextPrefab != null)
        {
            GameObject textObj = Instantiate(damageTextPrefab, transform.position + Vector3.up, Quaternion.identity);
            textObj.GetComponent<DamageNumber>().Setup(damage);
        }
    }

    void Die() => Destroy(gameObject);
}