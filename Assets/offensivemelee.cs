using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "NewMeleeAbility", menuName = "Abilities/Melee")]
public class offensivemelee : offensiveability
{
    public float spawnOffset = 1.0f;
    public float rotationOffset = 0f;

    [Header("Combo Burst Settings")]
    public int swingsPerAttack = 2;
    public float delayBetweenSwings = 0.15f;

    public override void Execute(Transform caster, Transform targetAnchor)
    {
        // Check if caster is still alive/exists
        if (caster == null) return;
        caster.GetComponent<MonoBehaviour>().StartCoroutine(MeleeSequence(caster, targetAnchor));
    }

    private IEnumerator MeleeSequence(Transform caster, Transform targetAnchor)
    {
        for (int i = 0; i < swingsPerAttack; i++)
        {
            // Safety check in case player is destroyed during sequence
            if (caster == null) yield break;

            PerformSingleSwing(caster, targetAnchor, i + 1);

            if (i < swingsPerAttack - 1)
            {
                yield return new WaitForSeconds(delayBetweenSwings);
            }
        }
    }

    private void PerformSingleSwing(Transform caster, Transform targetAnchor, int index)
    {
        if (prefab == null) return;

        // 1. Get Direction relative to current caster position
        Vector2 rawDir = (targetAnchor.position - caster.position).normalized;
        Vector2 snappedDir = Mathf.Abs(rawDir.x) > Mathf.Abs(rawDir.y)
            ? new Vector2(Mathf.Sign(rawDir.x), 0)
            : new Vector2(0, Mathf.Sign(rawDir.y));

        float angle = (Mathf.Atan2(snappedDir.y, snappedDir.x) * Mathf.Rad2Deg) + rotationOffset;

        // 2. Spawn and Parent immediately
        // We use caster.position to ensure it's glued to the player's current frame position
        Vector3 spawnPos = caster.position + (Vector3)(snappedDir * spawnOffset);

        GameObject woosh = ObjectPooler.Instance.GetPooledObject(prefab, spawnPos, Quaternion.Euler(0, 0, angle));

        // SetParent(caster, true) tells Unity to keep its local position relative to the player
        woosh.transform.SetParent(caster, true);
        woosh.transform.localPosition = (Vector3)(snappedDir * spawnOffset);

        var behav = woosh.GetComponent<meleebehav>();
        if (behav != null)
        {
            behav.Setup(damage, index);
        }
    }
}