using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class enemyHealth : MonoBehaviour, IDamageable
{
    [Header("Health Settings")]
    public float health = 100f;
    private float maxHealth;

    [Header("UI References")]
    public GameObject healthBarObject; // Drag the Canvas or the HealthBar Parent here
    public Image healthBarFill;       // Drag your Red 'Filled' Image here
    public GameObject damageTextPrefab;

    [Header("Visual Effects")]
    public Renderer enemyRenderer;
    public float flashDuration = 0.2f;
    private Material _material;
    private Coroutine _flashCoroutine;

    void Start()
    {
        maxHealth = health;

        if (enemyRenderer != null)
            _material = enemyRenderer.material;

        UpdateHealthUI();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);

        UpdateHealthUI();
        ShowDamageText(damage);

        if (_flashCoroutine != null) StopCoroutine(_flashCoroutine);
        _flashCoroutine = StartCoroutine(FlashEffect());

        if (health <= 0) Die();
    }

    void UpdateHealthUI()
    {
        // 1. Update the fill amount
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = health / maxHealth;
        }

        // 2. Hide if full, Show if damaged
        if (healthBarObject != null)
        {
            if (health >= maxHealth)
                healthBarObject.SetActive(false);
            else
                healthBarObject.SetActive(true);
        }
    }

    IEnumerator FlashEffect()
    {
        float elapsed = 0f;
        while (elapsed < flashDuration)
        {
            elapsed += Time.deltaTime;
            float intensity = Mathf.Lerp(1f, 0f, elapsed / flashDuration);
            if (_material != null) _material.SetFloat("_Intensity", intensity);
            yield return null;
        }
        if (_material != null) _material.SetFloat("_Intensity", 0f);
    }

    void ShowDamageText(float damage)
    {
        if (damageTextPrefab != null)
        {
            GameObject textObj = Instantiate(damageTextPrefab, transform.position + Vector3.up, Quaternion.identity);
            if (textObj.TryGetComponent<DamageNumber>(out DamageNumber dn))
            {
                dn.Setup(damage);
            }
        }
    }

    void Die() => Destroy(gameObject);
}