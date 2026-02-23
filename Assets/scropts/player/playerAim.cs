using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform anchor;
    public SpriteRenderer spriteRenderer;
    public charSO selectedChar;

    [Header("Settings")]
    public float maxDistance = 3f;
    public float smoothSpeed = 10f;

    void Start()
    {
        ApplyCharacterSprites();

        // Force initial facing direction (optional – ensures consistent start)
        if (player != null)
        {
            Vector3 scale = player.localScale;
            scale.x = Mathf.Abs(scale.x); // start facing right
            player.localScale = scale;
        }
    }

    void Update()
    {
        if (player == null || anchor == null || spriteRenderer == null)
        {
            Debug.LogWarning($"[PlayerAim] Missing references on {gameObject.name}");
            return;
        }

        AimAtMouse();
    }

    void ApplyCharacterSprites()
    {
        if (selectedChar == null || spriteRenderer == null) return;

        // Default sprite when game starts (facing down)
        spriteRenderer.sprite = selectedChar.frontsprite;
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
        // Swap sprite based on aiming direction
        // ======================
        if (selectedChar != null)
        {
            if (direction.y > 0f && selectedChar.backsprite != null)
                spriteRenderer.sprite = selectedChar.backsprite;
            else if (direction.y <= 0f && selectedChar.frontsprite != null)
                spriteRenderer.sprite = selectedChar.frontsprite;
        }

        // ======================
        // Flip horizontally based on direction
        // ======================

        Vector3 playerScale = player.localScale;

        // Prevent zero sign bug
        float xSign = direction.x >= 0 ? 1f : -1f;

        if (direction.y > 0f)
        {
            // LOOKING UP → normal flip
            playerScale.x = Mathf.Abs(playerScale.x) * xSign;
        }
        else
        {
            // LOOKING DOWN → inverted flip
            playerScale.x = Mathf.Abs(playerScale.x) * -xSign;
        }

        player.localScale = playerScale;
    }

    private void OnDrawGizmosSelected()
    {
        if (player != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(player.position, maxDistance);
        }
    }
}