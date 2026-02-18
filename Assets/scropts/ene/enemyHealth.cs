using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Pathfinding;

public class enemyHealth : MonoBehaviour, IDamageable
{
    [Header("Health Settings")]
    public float health = 100f;
    private float maxHealth;

    [Header("UI References")]
    public GameObject healthBarObject;
    public Image healthBarFill;
    public GameObject damageTextPrefab;

    [Header("Visual Effects")]
    public float flashDuration = 0.2f;
    private Coroutine _flashCoroutine;

    [Header("Slow Settings")]
    public Color slowColor = Color.yellow;

    private Color originalColor;
    private Coroutine slowCoroutine;

    private AIPath ai;
    private float originalSpeed;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        maxHealth = health;

        // Get SpriteRenderer
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;

        // Get A* AIPath
        ai = GetComponent<AIPath>();
        if (ai != null)
            originalSpeed = ai.maxSpeed;

        UpdateHealthUI();
    }

    // ======================
    // DAMAGE
    // ======================
    public void TakeDamage(float damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);

        UpdateHealthUI();
        ShowDamageText(damage);

        if (_flashCoroutine != null)
            StopCoroutine(_flashCoroutine);

        _flashCoroutine = StartCoroutine(FlashEffect());

        if (health <= 0)
            Die();
    }

    // ======================
    // APPLY SLOW
    // ======================
    public void ApplySlow(float slowPercent, float duration)
    {
        if (ai == null) return;

        if (slowCoroutine != null)
            StopCoroutine(slowCoroutine);

        slowCoroutine = StartCoroutine(SlowRoutine(slowPercent, duration));
    }

    IEnumerator SlowRoutine(float slowPercent, float duration)
    {
        // Apply speed reduction
        ai.maxSpeed = originalSpeed * (1f - slowPercent);

        // Apply yellow tint
        if (spriteRenderer != null)
            spriteRenderer.color = slowColor;

        yield return new WaitForSeconds(duration);

        // Restore speed
        ai.maxSpeed = originalSpeed;

        // Restore original color
        if (spriteRenderer != null)
            spriteRenderer.color = originalColor;
    }

    // ======================
    // FLASH EFFECT (simple white flash)
    // ======================
    IEnumerator FlashEffect()
    {
        if (spriteRenderer == null)
            yield break;

        Color flashColor = Color.white;
        spriteRenderer.color = flashColor;

        yield return new WaitForSeconds(flashDuration);

        // If still slowed, keep slow color
        if (slowCoroutine != null)
            spriteRenderer.color = slowColor;
        else
            spriteRenderer.color = originalColor;
    }

    // ======================
    // HEALTH UI
    // ======================
    void UpdateHealthUI()
    {
        if (healthBarFill != null)
            healthBarFill.fillAmount = health / maxHealth;

        if (healthBarObject != null)
            healthBarObject.SetActive(health < maxHealth);
    }

    // ======================
    // DAMAGE TEXT
    // ======================
    void ShowDamageText(float damage)
    {
        if (damageTextPrefab != null)
        {
            GameObject textObj = Instantiate(
                damageTextPrefab,
                transform.position + Vector3.up,
                Quaternion.identity
            );

            if (textObj.TryGetComponent<DamageNumber>(out DamageNumber dn))
                dn.Setup(damage);
        }
    }

    // ======================
    // DEATH
    // ======================
    void Die()
    {
        Destroy(gameObject);
    }
}
