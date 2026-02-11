using UnityEngine;
using System.Collections;

public class enemyHealth : MonoBehaviour, IDamageable
{
    public float health = 100f;
    public GameObject damageTextPrefab;

    [Header("Visual Effects")]
    public Renderer enemyRenderer; // Drag the mesh renderer here
    public float flashDuration = 0.2f;
    private Material _material;
    private Coroutine _flashCoroutine;

    void Start()
    {
        // Cache the material to avoid repeated calls
        if (enemyRenderer != null)
            _material = enemyRenderer.material;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        ShowDamageText(damage);

        // Trigger the flash effect
        if (_flashCoroutine != null) StopCoroutine(_flashCoroutine);
        _flashCoroutine = StartCoroutine(FlashEffect());

        if (health <= 0) Die();
    }

    IEnumerator FlashEffect()
    {
        float elapsed = 0f;

        while (elapsed < flashDuration)
        {
            elapsed += Time.deltaTime;
            // Linear fade from 1 to 0
            float intensity = Mathf.Lerp(1f, 0f, elapsed / flashDuration);
            _material.SetFloat("_Intensity", intensity);
            yield return null;
        }

        // Ensure it resets to exactly 0
        _material.SetFloat("_Intensity", 0f);
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