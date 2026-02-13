using UnityEngine;
using UnityEngine.InputSystem;

public class InputDebugTest : MonoBehaviour
{
    public void OnTest(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("SPACE PRESSED");
        }
    }
}
