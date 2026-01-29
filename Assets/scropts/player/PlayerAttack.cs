using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("References")]
    public charSO currentChar;
    public Transform anchor;

    private float nextFireTime = 0f;

    void Update()
    {
        // Safety check
        if (currentChar == null || currentChar.abilities.Length == 0) return;

        // Using the first ability in the array for now
        Ability currentAbility = currentChar.abilities[0];

        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            currentAbility.Execute(transform, anchor);
            nextFireTime = Time.time + currentAbility.fireRate;
        }
    }
}