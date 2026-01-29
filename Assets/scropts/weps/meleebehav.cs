using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class meleebehav : MonoBehaviour
{
    private float damage;
    private List<GameObject> hitEnemies = new List<GameObject>();
    private Animator anim;

    private void Awake() => anim = GetComponent<Animator>();

    public void Setup(float dmg, int swingIndex)
    {
        damage = dmg;
        hitEnemies.Clear();

        StopAllCoroutines();

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
        // Wait for the next fixed update or frame to ensure physics/animator are in sync
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
            Debug.Log($"Hit {collision.name}!");
        }
    }

    void Deactivate()
    {
        // Clear parenting BEFORE disabling to prevent the pooler from getting confused
        transform.SetParent(null);
        gameObject.SetActive(false);
    }
}