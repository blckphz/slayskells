using UnityEngine;

// This attribute lets you right-click in your folders to create new abilities

public abstract class Ability : ScriptableObject
{
    public float fireRate = 0.2f;
    public GameObject prefab;
    public Sprite icon;

    // We pass the "parent" Transform so the SO knows where the player is
    public abstract void Execute(Transform caster, Transform targetAnchor);
}