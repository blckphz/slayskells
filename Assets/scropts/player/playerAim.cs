using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    [Header("References")]
    public Transform player;        // Player transform
    public Transform anchor;        // Aim anchor
    public GameObject backSprite;   // Back sprite (shows when aiming up)
    public GameObject frontSprite;  // Front sprite (optional, shows normally)

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

        // Direction and clamp
        Vector3 direction = mousePos - player.position;
        if (direction.magnitude > maxDistance)
            direction = direction.normalized * maxDistance;

        Vector3 targetPos = player.position + direction;

        // Smoothly move anchor
        anchor.position = Vector3.Lerp(anchor.position, targetPos, smoothSpeed * Time.deltaTime);

        // ======================
        // Show back sprite when aiming up
        // ======================
        if (backSprite != null)
        {
            backSprite.SetActive(direction.y > 0f);
        }

        // Optional: show front sprite when aiming down
        if (frontSprite != null)
        {
            frontSprite.SetActive(direction.y <= 0f);
        }

        // ======================
        // Optional: Flip horizontally based on mouse X
        // ======================
        float scaleX = Mathf.Sign(direction.x);
        Vector3 playerScale = player.localScale;
        playerScale.x = Mathf.Abs(playerScale.x) * scaleX;
        player.localScale = playerScale;
    }

    // Draw aim range in editor
    private void OnDrawGizmosSelected()
    {
        if (player != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(player.position, maxDistance);
        }
    }
}
