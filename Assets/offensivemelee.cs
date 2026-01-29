using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "NewMeleeAbility", menuName = "Abilities/Melee")]
public class offensivemelee : offensiveability
{
    [Header("Placement Settings")]
    public float spawnOffset = 1.5f;
    public float rotationOffset = 0f;

    [Header("Combo Settings")]
    public int swingsPerAttack = 2;
    public float delayBetweenSwings = 0.15f;

    public override void Execute(Transform caster, Transform targetAnchor)
    {
        if (caster == null) return;
        caster.GetComponent<MonoBehaviour>().StartCoroutine(MeleeSequence(caster, targetAnchor));
    }

    private IEnumerator MeleeSequence(Transform caster, Transform targetAnchor)
    {
        for (int i = 0; i < swingsPerAttack; i++)
        {
            if (caster == null) yield break;

            PerformSingleSwing(caster, targetAnchor, i + 1);

            if (i < swingsPerAttack - 1)
                yield return new WaitForSeconds(delayBetweenSwings);
        }
    }

    private void PerformSingleSwing(Transform caster, Transform targetAnchor, int index)
    {
        if (prefab == null) return;

        // 1. Calculate direction (snap to cardinal)
        Vector2 rawDir = (targetAnchor.position - caster.position).normalized;
        Vector2 snappedDir = Mathf.Abs(rawDir.x) > Mathf.Abs(rawDir.y)
            ? new Vector2(Mathf.Sign(rawDir.x), 0)
            : new Vector2(0, Mathf.Sign(rawDir.y));

        // 2. Calculate rotation
        float angle = (Mathf.Atan2(snappedDir.y, snappedDir.x) * Mathf.Rad2Deg) + rotationOffset;

        // 3. Spawn from pool
        GameObject woosh = ObjectPooler.Instance.GetPooledObject(prefab, caster.position, Quaternion.Euler(0, 0, angle));

        // 4. Parenting & offset
        woosh.transform.SetParent(caster);
        woosh.transform.localPosition = (Vector3)(snappedDir * spawnOffset);

        // 5. Setup melee behavior
        var behav = woosh.GetComponent<meleebehav>();
        if (behav != null)
            behav.Setup(damage, index);

        // 6. Camera shake per swing
        CameraShaker.Shake(0.4f, 0.12f);

        Debug.DrawRay(caster.position, (Vector3)snappedDir * spawnOffset, Color.magenta, 0.4f);
    }
}
