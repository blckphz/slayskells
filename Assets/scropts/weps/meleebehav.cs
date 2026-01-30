using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class meleebehav : MonoBehaviour
{
    private float damage;
    private List<IDamageable> hitEnemies = new List<IDamageable>();
    private Animator anim;
    private Vector3 prefabScale;

    private Transform spriteTransform;
    private Coroutine deactivationRoutine;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        prefabScale = transform.localScale;

        spriteTransform = GetComponentInChildren<SpriteRenderer>()?.transform;
    }

    public void Setup(float dmg, int swingIndex)
    {
        damage = dmg;
        hitEnemies.Clear();
        StopAllCoroutines();

        // Flip every second swing for visual variety
        bool isEven = (swingIndex % 2 == 0);
        transform.localScale = new Vector3(
            isEven ? -prefabScale.x : prefabScale.x,
            prefabScale.y,
            prefabScale.z
        );

        if (anim != null)
        {
            anim.SetInteger("SwingIndex", swingIndex);
            anim.SetTrigger("Attack");
            StartCoroutine(DeactivateAfterAnimation());
        }
        else
        {
            // fallback if no animator
            if (deactivationRoutine != null) StopCoroutine(deactivationRoutine);
            deactivationRoutine = StartCoroutine(DeactivateAfterTime(0.3f));
        }
    }

    private IEnumerator DeactivateAfterAnimation()
    {
        yield return new WaitForEndOfFrame();

        if (anim != null)
        {
            float duration = anim.GetCurrentAnimatorStateInfo(0).length;
            yield return new WaitForSeconds(duration);
        }

        Deactivate();
    }

    private IEnumerator DeactivateAfterTime(float delay)
    {
        yield return new WaitForSeconds(delay);
        Deactivate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable target = collision.GetComponent<IDamageable>();
        if (target != null && !hitEnemies.Contains(target))
        {
            // Apply damage
            target.TakeDamage(damage);
            hitEnemies.Add(target);

            // Camera shake per enemy hit
            CameraShaker.Shake(0.35f, 0.12f);
        }
    }

    void Deactivate()
    {
        transform.localScale = prefabScale;
        transform.SetParent(null);
        gameObject.SetActive(false);
    }
}
