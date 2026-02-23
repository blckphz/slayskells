using UnityEngine;

public class grenadeBehav : MonoBehaviour
{
    private float damage;
    private float radius;
    public float fuseTime = 2.5f;
    public GameObject explosionEffect;

    private bool hasExploded = false;

    // Static reference for the currently active grenade instance
    public static grenadeBehav ActiveGrenade;

    // Static reference to the grenade SO for spawning
    public static grenadeSO ActiveGrenadeSO;

    public void Initialize(float dmg, float rad, float fuse = 2.5f)
    {
        damage = dmg;
        radius = rad;
        fuseTime = fuse;
        hasExploded = false;

        ActiveGrenade = this; // register active grenade

        Invoke(nameof(Explode), fuseTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasExploded) return;

        IDamageable target = collision.GetComponent<IDamageable>();
        if (target != null)
        {
            Explode();
        }
    }

    public void ManualExplode()
    {
        if (hasExploded) return;
        Explode();
    }

    void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;
        CancelInvoke(nameof(Explode));


        Collider2D[] objectsInRange = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (Collider2D obj in objectsInRange)
        {
            IDamageable target = obj.GetComponent<IDamageable>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }

            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 dir = (Vector2)obj.transform.position - (Vector2)transform.position;
                rb.AddForce(dir.normalized * 5f, ForceMode2D.Impulse);
            }
        }

        if (explosionEffect)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        if (ActiveGrenade == this) ActiveGrenade = null;

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    // =========================
    // Static Helper: Spawn or Explode
    // =========================
    public static void SpawnOrExplode(Transform spawnPoint, Transform targetAnchor, float damage, float radius, float fuseTime = 2.5f, float throwForce = 10f)
    {
        if (ActiveGrenade != null)
        {
            ActiveGrenade.ManualExplode();
        }
        else if (ActiveGrenadeSO != null)
        {
            Debug.Log("[Grenade] Static call: Spawning new grenade");
            GameObject grenadeGO = Object.Instantiate(ActiveGrenadeSO.prefab, spawnPoint.position, Quaternion.identity);
            if (grenadeGO.TryGetComponent(out grenadeBehav logic))
            {
                logic.Initialize(damage, radius, fuseTime);

                Rigidbody2D rb = grenadeGO.GetComponent<Rigidbody2D>();
                if (rb != null && targetAnchor != null)
                {
                    Vector2 throwDir = (targetAnchor.position - spawnPoint.position).normalized;
                    rb.AddForce(throwDir * throwForce, ForceMode2D.Impulse);
                    Debug.Log("[Grenade] Applied throw force via static spawn");
                }
            }
        }
        else
        {
            Debug.LogWarning("[Grenade] ActiveGrenadeSO is not set! Cannot spawn grenade.");
        }
    }
}
