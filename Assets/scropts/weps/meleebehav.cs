using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class meleebehav : MonoBehaviour
{
    private float damage;
    private List<GameObject> hitEnemies = new List<GameObject>();
    private Animator anim;
    private Vector3 prefabScale;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        prefabScale = transform.localScale;
    }

    public void Setup(float dmg, int swingIndex)
    {
        damage = dmg;
        hitEnemies.Clear();
        StopAllCoroutines();

        // Flip every second swing for variety
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
            Invoke(nameof(Deactivate), 0.3f);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && !hitEnemies.Contains(collision.gameObject))
        {
            hitEnemies.Add(collision.gameObject);

            // Apply damage logic
            Debug.Log($"Hit {collision.name} for {damage} damage!");

            // Optional: shake camera per enemy hit instead of per swing
            // CameraShaker.Shake(0.35f, 0.12f);
        }
    }

    void Deactivate()
    {
        transform.localScale = prefabScale;
        transform.SetParent(null);
        gameObject.SetActive(false);
    }
}
