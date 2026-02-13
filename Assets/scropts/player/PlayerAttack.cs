using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("References")]
    public charSO currentChar;

    [Header("Settings")]
    [SerializeField] private float defaultShakeIntensity = 0.5f;

    // Internal tracker for cooldowns
    public Dictionary<Ability, float> abilityCooldowns = new Dictionary<Ability, float>();

    // LIVE ANCHOR PROPERTY: Always gets the current position of the anchor from PlayerAim
    public Transform CurrentAnchor
    {
        get
        {
            PlayerAim aim = GameObject.FindObjectOfType<PlayerAim>();
            if (aim != null && aim.anchor != null)
            {
                return aim.anchor;
            }
            return null;
        }
    }

    void Update()
    {
        if (currentChar == null || currentChar.abilities == null || currentChar.abilities.Length == 0)
            return;

        // Ability 1 (Left Click)
        if (Input.GetButton("Fire1"))
        {
            TryUseAbility(0);
        }

        // Ability 2 (Right Click)
        if (currentChar.abilities.Length > 1 && Input.GetButton("Fire2"))
        {
            TryUseAbility(1);
        }

        // Ability 3 (Middle Click / Fire4)
        if (currentChar.abilities.Length > 2 && Input.GetButton("Fire4"))
        {
            TryUseAbility(2);
        }

        if (currentChar.abilities.Length > 2 && Input.GetButton("Fire5"))
        {
            TryUseAbility(3);
        }
    }

    private void TryUseAbility(int index)
    {
        Debug.Log($"Trying to use ability index {index}");

        if (currentChar == null || currentChar.abilities == null)
            return;

        if (index >= currentChar.abilities.Length)
            return;

        Ability ability = currentChar.abilities[index];

        if (ability != null && CanUseAbility(ability))
        {
            Debug.Log($"Using ability: {ability.name}");
            PerformAttack(ability);
        }
    }

    private bool CanUseAbility(Ability ability)
    {
        if (!abilityCooldowns.ContainsKey(ability))
            abilityCooldowns[ability] = 0f;

        bool ready = Time.time >= abilityCooldowns[ability];

        // Uncomment the line below if you want to see cooldown status in console
        // if (!ready) Debug.Log($"[PlayerAttack] {ability.name} on cooldown for {abilityCooldowns[ability] - Time.time:F2}s");

        return ready;
    }

    private void PerformAttack(Ability ability)
    {
        Transform activeAnchor = CurrentAnchor;

        // Safety check for the anchor
        if (activeAnchor == null)
        {
            Debug.LogError($"[PlayerAttack] Cannot attack! No Anchor found on the PlayerAim script of {gameObject.name}");
            return;
        }

        Debug.Log($"[PlayerAttack] Executing {ability.name} at Position: {activeAnchor.position}");

        // 1. Audio
        if (ability.launchsound != null && audiomanager.Instance != null)
        {
            audiomanager.Instance.PlaySound(ability.launchsound);
        }

        // 2. Logic - Uses the LIVE position of the anchor
        ability.Execute(transform, activeAnchor);

        // 3. Set Cooldown
        abilityCooldowns[ability] = Time.time + ability.fireRate;

        // 4. UI Trigger
        if (charsetter.Instance != null)
        {
            charsetter.Instance.TriggerAbilityUsed(ability);
        }
        else
        {
            Debug.LogWarning("[PlayerAttack] charsetter.Instance is missing! UI won't update.");
        }
    }
}