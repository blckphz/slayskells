using UnityEngine;
using System.Collections.Generic;

public class PlayerAttack : MonoBehaviour
{
    [Header("References")]
    public charSO currentChar;
    public Transform anchor;

    [Header("Settings")]
    [Tooltip("Default shake if the ability doesn't have a value")]
    [SerializeField] private float defaultShakeIntensity = 0.5f;

    // Track cooldowns per ability
    private Dictionary<Ability, float> abilityCooldowns = new Dictionary<Ability, float>();

    void Update()
    {
        if (currentChar == null || currentChar.abilities == null || currentChar.abilities.Length == 0)
            return;

        // Fire1 (Primary) → first ability
        Ability primary = currentChar.abilities[0];
        if (primary != null && Input.GetButton("Fire1") && CanUseAbility(primary))
        {
            PerformAttack(primary);
        }

        // Fire2 (Secondary) → second ability if exists
        Ability secondary = currentChar.abilities.Length > 1 ? currentChar.abilities[1] : null;
        if (secondary != null && Input.GetButton("Fire2") && CanUseAbility(secondary))
        {
            PerformAttack(secondary);
        }
    }

    private bool CanUseAbility(Ability ability)
    {
        if (!abilityCooldowns.ContainsKey(ability))
            abilityCooldowns[ability] = 0f;

        return Time.time >= abilityCooldowns[ability];
    }

    private void PerformAttack(Ability ability)
    {
        if (ability == null) return;

        Debug.Log($"<color=white><b>[Input] {ability.name} Fired</b></color>");

        // Execute ability
        ability.Execute(transform, anchor);

        // Set this ability's cooldown independently
        abilityCooldowns[ability] = Time.time + ability.fireRate;
    }
}
