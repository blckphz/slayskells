using UnityEngine;
using System.Collections;

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

            // Play sound for every individual swing
            if (launchsound != null)
            {
                audiomanager.Instance?.PlaySound(launchsound);
            }

            PerformSingleSwing(caster, targetAnchor, i + 1);

            if (i < swingsPerAttack - 1)
                yield return new WaitForSeconds(delayBetweenSwings);
        }
    }

    private void PerformSingleSwing(Transform caster, Transform targetAnchor, int index)
    {
        if (prefab == null) return;

        Vector2 rawDir = (targetAnchor.position - caster.position).normalized;
        Vector2 snappedDir = Mathf.Abs(rawDir.x) > Mathf.Abs(rawDir.y)
            ? new Vector2(Mathf.Sign(rawDir.x), 0)
            : new Vector2(0, Mathf.Sign(rawDir.y));

        float angle = (Mathf.Atan2(snappedDir.y, snappedDir.x) * Mathf.Rad2Deg) + rotationOffset;

        GameObject woosh = ObjectPooler.Instance.GetPooledObject(prefab, caster.position, Quaternion.Euler(0, 0, angle));

        woosh.transform.SetParent(caster);
        woosh.transform.localPosition = (Vector3)(snappedDir * spawnOffset);

        var behav = woosh.GetComponent<meleebehav>();
        if (behav != null)
            behav.Setup(damage, index);

        CameraShaker.Shake(0.4f, 0.12f);
    }
}