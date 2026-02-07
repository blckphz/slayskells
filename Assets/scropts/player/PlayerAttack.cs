using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerAttack : MonoBehaviour
{
    [Header("References")]
    public charSO currentChar;
    public Transform anchor;

    [Header("Settings")]
    [Tooltip("Default shake if the ability doesn't have a value")]
    [SerializeField] private float defaultShakeIntensity = 0.5f;

    // Track cooldowns per ability
    public Dictionary<Ability, float> abilityCooldowns = new Dictionary<Ability, float>();

    void Update()
    {
        // Safety check to ensure we have a character and abilities assigned
        if (currentChar == null || currentChar.abilities == null || currentChar.abilities.Length == 0)
            return;

        // --- Ability 1 (Left Click / Fire1) ---
        Ability primary = currentChar.abilities[0];
        if (primary != null && UnityEngine.Input.GetButton("Fire1") && CanUseAbility(primary))
        {
            PerformAttack(primary);
        }

        // --- Ability 2 (Right Click / Fire2) ---
        if (currentChar.abilities.Length > 1)
        {
            Ability secondary = currentChar.abilities[1];
            if (secondary != null && UnityEngine.Input.GetButton("Fire2") && CanUseAbility(secondary))
            {
                PerformAttack(secondary);
            }
        }

        // --- Ability 3 (Middle Click / Fire3) ---
        if (currentChar.abilities.Length > 2)
        {
            Ability special = currentChar.abilities[2];
            if (special != null && UnityEngine.Input.GetButton("Fire4") && CanUseAbility(special))
            {
                PerformAttack(special);
            }
        }
    }

    private bool CanUseAbility(Ability ability)
    {
        // Initialize the cooldown entry if it doesn't exist yet
        if (!abilityCooldowns.ContainsKey(ability))
            abilityCooldowns[ability] = 0f;

        // Check if the current time has passed the stored cooldown timestamp
        return Time.time >= abilityCooldowns[ability];
    }

    private void PerformAttack(Ability ability)
    {
        if (ability == null) return;

        // 1. Play the launch sound via the AudioManager
        if (ability.launchsound != null && audiomanager.Instance != null)
        {
            audiomanager.Instance.PlaySound(ability.launchsound);
        }

        // 2. Execute the ability logic (passes the player's transform and the aim anchor)
        ability.Execute(transform, anchor);

        // 3. Set the next available time this ability can be used
        abilityCooldowns[ability] = Time.time + ability.fireRate;

        // 4. Trigger UI effects (Cooldown overlays, icon bounces, etc.)
        charsetter.Instance?.TriggerAbilityUsed(ability);
    }
}

