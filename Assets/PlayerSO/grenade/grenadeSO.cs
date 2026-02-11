using UnityEngine;

[CreateAssetMenu(fileName = "New 2D Grenade", menuName = "Abilities/2D Grenade")]
public class grenadeSO : offensiveRanged
{
    public float throwForce = 10f;
    public float explosionRadius = 3f;

    public override void Execute(Transform caster, Transform targetAnchor)
    {
        // 1. Spawn the grenade
        GameObject grenade = Instantiate(prefab, caster.position, Quaternion.identity);

        // 2. Calculate 2D Direction
        Vector2 throwDir = (targetAnchor.position - caster.position).normalized;

        // 3. Apply 2D Physics
        Rigidbody2D rb = grenade.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(throwDir * throwForce, ForceMode2D.Impulse);
            // Add a little bit of torque for a nice spin effect
            //rb.AddTorque(5f, ForceMode2D.Impulse);
        }

        // 4. Pass the stats to the prefab's logic script
        if (grenade.TryGetComponent(out grenadeBehav logic))
        {
            logic.Initialize(damage, explosionRadius);
        }
    }
}