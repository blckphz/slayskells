using UnityEngine;

[CreateAssetMenu(fileName = "New 2D Grenade", menuName = "Abilities/2D Grenade")]
public class grenadeSO : offensiveRanged
{
    public float throwForce = 10f;
    public float explosionRadius = 3f;

    public override void Execute(Transform caster, Transform targetAnchor)
    {
        Debug.Log("[GrenadeSO] Executing grenade...");

        // Spawn grenade prefab at caster position
        GameObject grenade = Instantiate(prefab, caster.position, Quaternion.identity);

        // Calculate 2D throw direction
        Vector2 throwDir = (targetAnchor.position - caster.position).normalized;

        // Apply physics
        Rigidbody2D rb = grenade.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(throwDir * throwForce, ForceMode2D.Impulse);
            Debug.Log($"[GrenadeSO] Thrown with force {throwForce}");
        }

        // Initialize grenade logic
        if (grenade.TryGetComponent(out grenadeBehav logic))
        {
            logic.Initialize(damage, explosionRadius, logic.fuseTime);
            Debug.Log("[GrenadeSO] Grenade logic initialized");
        }
    }
}
