using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI; // Required for Slider

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float dashSpeed = 12f;
    public Rigidbody2D rb;

    [Header("Stamina Settings")]
    public float maxStamina = 1.5f;
    public float rechargeRate = 0.5f;
    public float emptyPenaltyTime = 1f;

    [Header("UI Components")]
    public Slider staminaSlider;
    public Image fillImage; // The "Fill" part of the slider
    public Color normalColor = Color.yellow;
    public Color exhaustedColor = Color.red;

    private Vector2 moveInput;
    private bool isDashButtonHeld;
    private float currentStamina;
    private bool isExhausted;

    void Awake()
    {
        currentStamina = maxStamina;

        // Initialize UI
        if (staminaSlider != null)
        {
            staminaSlider.maxValue = maxStamina;
            staminaSlider.value = maxStamina;
        }
    }

    void OnMove(InputValue value) => moveInput = value.Get<Vector2>();
    void OnDash(InputValue value) => isDashButtonHeld = value.isPressed;

    void Update()
    {
        // Update UI every frame for smoothness
        if (staminaSlider != null)
        {
            staminaSlider.value = currentStamina;

            // Change color if exhausted
            if (fillImage != null)
                fillImage.color = isExhausted ? exhaustedColor : normalColor;
        }
    }

    void FixedUpdate()
    {
        HandleStamina();

        bool isMoving = moveInput != Vector2.zero;
        bool canDash = isDashButtonHeld && isMoving && !isExhausted && currentStamina > 0;

        float currentSpeed = canDash ? dashSpeed : moveSpeed;
        rb.MovePosition(rb.position + moveInput * currentSpeed * Time.fixedDeltaTime);
    }

    private void HandleStamina()
    {
        bool isMoving = moveInput != Vector2.zero;

        if (isDashButtonHeld && isMoving && !isExhausted)
        {
            currentStamina -= Time.fixedDeltaTime;
            if (currentStamina <= 0)
            {
                currentStamina = 0;
                isExhausted = true;
                Invoke(nameof(ResetExhaustion), emptyPenaltyTime);
            }
        }
        else
        {
            if (currentStamina < maxStamina)
                currentStamina += Time.fixedDeltaTime * rechargeRate;

            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

            // Recover from exhaustion at 20%
            if (isExhausted && currentStamina >= (maxStamina * 0.2f))
                isExhausted = false;
        }
    }

    private void ResetExhaustion()
    {
        // This ensures they can't dash again until the 20% check is also met
    }
}