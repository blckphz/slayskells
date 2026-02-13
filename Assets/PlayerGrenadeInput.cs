using UnityEngine;

public class PlayerGrenadeInput : MonoBehaviour
{
    [Header("References")]
    public grenadeSO grenadeAbility;

    [Header("Settings")]
    public KeyCode grenadeKey = KeyCode.G;

    void Update()
    {
        if (Input.GetKeyDown(grenadeKey))
        {
            HandleGrenadeAction();
        }
    }

    void HandleGrenadeAction()
    {
        // Check if a grenade already exists in the scene
        if (grenadeBehav.ActiveGrenade != null)
        {
            Debug.Log("[Input] Detonating active grenade!");
            grenadeBehav.ActiveGrenade.ManualExplode();
        }
    }
}