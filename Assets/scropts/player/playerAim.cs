using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    public Transform player;         // The player object
    public Transform anchor;         // The anchor/crosshair object
    public float maxDistance = 3f;   // Maximum distance from player
    public float smoothSpeed = 10f;  // Speed of smoothing

    void Update()
    {
        AimAtMouse();
    }

    void AimAtMouse()
    {
        // Get mouse position in world space
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f; // Ensure it's 2D

        // Calculate direction from player to mouse
        Vector3 direction = mousePos - player.position;

        // Limit the distance
        if (direction.magnitude > maxDistance)
        {
            direction = direction.normalized * maxDistance;
        }

        // Target position for the anchor
        Vector3 targetPos = player.position + direction;

        // Smoothly move anchor towards target position
        anchor.position = Vector3.Lerp(anchor.position, targetPos, smoothSpeed * Time.deltaTime);
    }
}
