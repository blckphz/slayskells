using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform anchor; // THE ONLY ANCHOR SLOT IN THE INSPECTOR

    [Header("Settings")]
    public float maxDistance = 3f;
    public float smoothSpeed = 10f;

    void Update()
    {
        if (player == null || anchor == null)
        {
            Debug.LogWarning($"[PlayerAim] Missing references on {gameObject.name}. Player: {player}, Anchor: {anchor}");
            return;
        }

        AimAtMouse();
    }

    void AimAtMouse()
    {
        if (Camera.main == null) return;

        // Convert mouse position to world space
        Vector3 mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Mathf.Abs(Camera.main.transform.position.z);
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        mousePos.z = 0f;

        // Calculate direction and clamp distance
        Vector3 direction = mousePos - player.position;
        if (direction.magnitude > maxDistance)
            direction = direction.normalized * maxDistance;

        Vector3 targetPos = player.position + direction;

        // Move anchor smoothly
        anchor.position = Vector3.Lerp(
            anchor.position,
            targetPos,
            smoothSpeed * Time.deltaTime
        );
    }

    // Draws a line in the editor so you can see the aim range
    private void OnDrawGizmosSelected()
    {
        if (player != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(player.position, maxDistance);
        }
    }
}